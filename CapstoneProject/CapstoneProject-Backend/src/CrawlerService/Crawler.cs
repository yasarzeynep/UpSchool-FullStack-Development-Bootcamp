using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using OfficeOpenXml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Application.Features.OrderEvents.Commands.Add;
using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Commands.Update;
using Application.Features.Products.Commands.Add;
using Application.Common.Dtos;
using Domain.Entities;
using Domain.Enums;
using System.ComponentModel;
using System.Net.Mail;
using LicenseContext = OfficeOpenXml.LicenseContext;
using Crawler;

const string HubUrl = "https://localhost:7245/Hubs/SeleniumLogHub";
const string OrdersUrl = "https://localhost:7245/api/Orders/Add";
const string OrdersUpdateUrl = "https://localhost:7245/api/Orders/Update";
const string OrdersEventsUrl = "https://localhost:7245/api/OrderEvents/Add";
const string ProductsUrl = "https://localhost:7245/api/Products/Add";
const string BaseUrl = "https://4teker.net/";

using var httpClient = new HttpClient();
List<Product> productsList = new List<Product>();

var hubConnection = new HubConnectionBuilder()
    .WithUrl(HubUrl)
    .WithAutomaticReconnect()
    .Build();

await hubConnection.StartAsync();

SeleniumLogDto CreateLog(string message) => new SeleniumLogDto(message);

while (true)
{

    Messages.PrintWelcomeMessage();
    Console.WriteLine("How many items do you want to engrave? (You can give a number or write 'all'.)");
    var requestedAmount = Console.ReadLine();

    Console.WriteLine("------------------------------------------------");
    Console.WriteLine("What type of products do you want to scrape?");
    Console.WriteLine("You can choose one option from three options");
    Console.WriteLine("A= All Products, B= On Sale Products, C= Regular Price Products");

    var productCrawlType = Console.ReadLine();

    Console.WriteLine("Do you want the scraped products as a result of the application to be transferred to you e-mail ? ");
    Console.WriteLine("Yes=Y or No=N");
    var sendEmail = Console.ReadLine().ToUpper();


    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("User preferences received."));

    var orderAddRequest = new OrderAddCommand();

    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Order received/requested."));

    bool validAnswer = false;

    while (!validAnswer)
    {
        switch (productCrawlType.ToUpper())
        {
            case "A":
                orderAddRequest = new OrderAddCommand()
                {
                    Id = Guid.NewGuid(),
                    ProductCrawlType = ProductCrawlType.All,
                    CreatedOn = DateTimeOffset.Now,
                    TotalFoundAmount = 0,
                    RequestedAmount = 0,
                };
                validAnswer = true;
                break;
            case "B":
                orderAddRequest = new OrderAddCommand()
                {
                    Id = Guid.NewGuid(),
                    ProductCrawlType = ProductCrawlType.OnDiscount,
                    CreatedOn = DateTimeOffset.Now,
                    TotalFoundAmount = 0,
                    RequestedAmount = 0,
                };
                validAnswer = true;
                break;
            case "C":
                orderAddRequest = new OrderAddCommand()
                {
                    Id = Guid.NewGuid(),
                    ProductCrawlType = ProductCrawlType.NonDiscount,
                    CreatedOn = DateTimeOffset.Now,
                    TotalFoundAmount = 0,
                    RequestedAmount = 0,
                };
                validAnswer = true;
                break;
            default:
                Console.WriteLine("Invalid option!");
                Thread.Sleep(1500);
                Console.Clear();
                break;

                await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog(OrderStatus.CrawlingFailed.ToString()));
        }
    }

    var orderAddResponse = await SendHttpPostRequest<OrderAddCommand, object>(httpClient, OrdersUrl, orderAddRequest);
    Guid orderId = orderAddRequest.Id;

    // Ayarlar ve yönlendirme

    ChromeOptions options = new ChromeOptions();
    options.AddArgument("--start-maximized");
    options.AddArgument("--disable-notifications");
    options.AddArgument("--disable-popup-blocking");

    var Driver = new ChromeDriver(options);
    var Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

    Console.Clear();

    Driver.Navigate().GoToUrl(BaseUrl);

    var orderEventAddRequest = new OrderEventAddCommand()
    {
        OrderId = orderId,
        Status = OrderStatus.BotStarted,
    };

    var orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, OrdersEventsUrl, orderEventAddRequest);

    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog(OrderStatus.BotStarted.ToString()));

    IWebElement pageCountElement = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".pagination > li:nth-last-child(2) > a")));
    int pageCount = int.Parse(pageCountElement.Text);

    Console.WriteLine($"{pageCount} number of pages available.");
    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"{pageCount} number of pages available."));
    Console.WriteLine("---------------------------------------");

    int itemCount = 0;

    orderEventAddRequest = new OrderEventAddCommand()
    {
        OrderId = orderId,
        Status = OrderStatus.CrawlingStarted,
    };

    orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, OrdersEventsUrl, orderEventAddRequest);

    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog(OrderStatus.CrawlingStarted.ToString()));

    for (int i = 1; i <= pageCount; i++)
    {
        Driver.Navigate().GoToUrl($"https://4teker.net/?currentPage={i}");

        Console.WriteLine($"{i}. Page");

        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"{i}.page was scanned."));

        WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

        Thread.Sleep(500);

        IReadOnlyCollection<IWebElement> productElements = Driver.FindElements(By.CssSelector(".card.h-100"));

        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"{productElements.Count} products found."));

        foreach (IWebElement productElement in productElements)
        {
            bool includeProduct = false;

            if (productCrawlType.ToUpper() == "A") // All option
            {
                includeProduct = true;
            }
            else if (productCrawlType.ToUpper() == "B") // Discounted products option
            {
                if (productElement.FindElements(By.CssSelector(".sale-price")).Any())
                    includeProduct = true;
            }
            else if (productCrawlType.ToUpper() == "C") // Regular priced products option
            {
                if (!productElement.FindElements(By.CssSelector(".sale-price")).Any())
                    includeProduct = true;
            }

            if (includeProduct)
            {
                // Get as many items as the user wants
                if (requestedAmount.ToLower() == "all" || itemCount < int.Parse(requestedAmount))
                {
                    string productName = productElement.FindElement(By.CssSelector(".fw-bolder.product-name")).GetAttribute("innerText");
                    string productPrice = productElement.FindElement(By.CssSelector(".price")).GetAttribute("innerText");
                    productPrice = productPrice.Replace("$", "").Replace(",", ".").Trim();
                    decimal price = decimal.Parse(productPrice, CultureInfo.InvariantCulture);
                    string productSalePrice = string.Empty;
                    IWebElement salePriceElement = null;

                    try
                    {
                        salePriceElement = productElement.FindElement(By.CssSelector(".sale-price"));
                    }
                    catch (NoSuchElementException)
                    {

                    }

                    decimal salePrice = 0;

                    if (salePriceElement != null)
                    {
                        productSalePrice = salePriceElement.GetAttribute("innerText");
                        productSalePrice = productSalePrice.Replace("$", "").Replace(",", ".").Trim();
                        salePrice = decimal.Parse(productSalePrice, CultureInfo.InvariantCulture);
                    }

                    bool isOnSale = productElement.FindElements(By.CssSelector(".sale-price")).Count > 0;
                    string pictureUrl = productElement.FindElement(By.CssSelector(".card-img-top")).GetAttribute("src");

                    Console.WriteLine("Product Name: " + productName);
                    Console.WriteLine("Is On Sale?: " + isOnSale);

                    if (isOnSale)
                    {
                        Console.WriteLine("Sale Price: " + salePrice);
                    }
                    else
                    {
                        Console.WriteLine("Price: No discount!");
                    }

                    Console.WriteLine("Price: " + price);
                    Console.WriteLine("Picture: " + pictureUrl);
                    Console.WriteLine("----------------------------");

                    var productAddRequest = new ProductAddCommand()
                    {
                        OrderId = orderAddRequest.Id,
                        Name = productName,
                        Picture = pictureUrl,
                        IsOnSale = isOnSale,
                        Price = price,
                        SalePrice = salePrice,
                        CreatedOn = DateTimeOffset.Now
                    };

                    var product = new Product()
                    {
                        OrderId = orderAddRequest.Id,
                        Name = productName,
                        Picture = pictureUrl,
                        IsOnSale = isOnSale,
                        Price = price,
                        SalePrice = salePrice,
                        CreatedOn = DateTimeOffset.Now
                    };

                    var productAddResponse = await SendHttpPostRequest<ProductAddCommand, object>(httpClient, ProductsUrl, productAddRequest);

                    productsList.Add(product);

                    // ProductLog

                    await hubConnection.InvokeAsync("SendProductLogNotificationAsync", CreateLog($"Product Name : {productName}" + "   |    " +
                    $"Is On Sale ? :   {isOnSale}" + "   |    " +
                        $"Product Price :   {price}" + "   |    " +
                        $"Product Sale Price :   {salePrice}"));

                    itemCount++;
                }
            }

            var orderUpdateRequest = new OrderUpdateCommand()
            {
                Id = orderId,
                TotalFoundAmount = itemCount,
                RequestedAmount = requestedAmount,
            };

            var orderUpdateResponse = await SendHttpPostRequest<OrderUpdateCommand, object>(httpClient, OrdersUpdateUrl, orderUpdateRequest);
        }
    }

    Driver.Dispose();

    Console.WriteLine($"{itemCount} products found.");
    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"{itemCount} products of the requested type were found"));

    orderEventAddRequest = new OrderEventAddCommand()
    {
        OrderId = orderId,
        Status = OrderStatus.CrawlingCompleted,
    };

    orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, OrdersEventsUrl, orderEventAddRequest);

    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog(OrderStatus.CrawlingCompleted.ToString()));

    orderEventAddRequest = new OrderEventAddCommand()
    {
        OrderId = orderId,
        Status = OrderStatus.OrderCompleted,
    };

    orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, OrdersEventsUrl, orderEventAddRequest);

    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog(OrderStatus.OrderCompleted.ToString()));

    if (sendEmail == "Y")
    {
        Console.WriteLine("Please enter a valid e-mail address!");

        var userEmail = Console.ReadLine();

        ExportToExcel(productsList, userEmail);
    }

    Console.WriteLine("Do you want to continue product scraping? (Y/N)");
    string answerContinue = Console.ReadLine().ToUpper();

    if (answerContinue == "N")
    {
        httpClient.Dispose();
        await hubConnection.StopAsync();
        break;
    }
}

async Task<TResponse> SendHttpPostRequest<TRequest, TResponse>(HttpClient httpClient, string url, TRequest payload)
{
    var jsonPayload = JsonConvert.SerializeObject(payload);
    var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
    var response = await httpClient.PostAsync(url, httpContent);
    response.EnsureSuccessStatusCode();
    var jsonResponse = await response.Content.ReadAsStringAsync();
    var responseObject = JsonConvert.DeserializeObject<TResponse>(jsonResponse);
    return responseObject;
}

void ExportToExcel(List<Product> products, string recipientEmail)
{
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

    using (var package = new ExcelPackage())
    {
        var worksheet = package.Workbook.Worksheets.Add("Products");
        worksheet.Cells["A1"].LoadFromCollection(products, true);
        var range = worksheet.Cells[worksheet.Dimension.Address];
        range.AutoFitColumns();

        var fileStream = new MemoryStream(package.GetAsByteArray());

        string subject = "Excel file";
        string body = "Hello, there are products in the attached Excel file.";

        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        mail.From = new MailAddress("your-email@gmail.com");
        mail.To.Add(recipientEmail);
        mail.Subject = subject;
        mail.Body = body;

        System.Net.Mail.Attachment attachment;
        attachment = new System.Net.Mail.Attachment(fileStream, "products.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        mail.Attachments.Add(attachment);

        SmtpServer.Port = 587;
        SmtpServer.Credentials = new System.Net.NetworkCredential("your-email@gmail.com", "your-email-password");
        SmtpServer.EnableSsl = true;

        SmtpServer.Send(mail);
    }
}

