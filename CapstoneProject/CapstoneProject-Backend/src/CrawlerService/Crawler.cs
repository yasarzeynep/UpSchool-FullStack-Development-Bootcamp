using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Mail;
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
using LicenseContext = OfficeOpenXml.LicenseContext;
using Crawler;
using System.Drawing;

#region Constants
const string hubUrl = "https://localhost:7294/Hubs/SeleniumLogHub";
const string ordersUrl = "https://localhost:7294/api/Orders/Add";
const string ordersUpdateUrl = "https://localhost:7294/api/Orders/Update";
const string ordersEventsUrl = "https://localhost:7294/api/OrderEvents/Add";
const string productsUrl = "https://localhost:7294/api/Products/Add";
const string baseUrl = "https://4teker.net/";
#endregion
// HttpClient and Product List
using var httpClient = new HttpClient();
List<Product> productsList = new List<Product>();

#region SignalR Hub Connection
var hubConnection = new HubConnectionBuilder()
    .WithUrl(hubUrl)
    .WithAutomaticReconnect()
    .Build();
#endregion

await hubConnection.StartAsync();

#region Helper method to create a log message
SeleniumLogDto CreateLog(string message, Guid id) => new SeleniumLogDto(message, id);
#endregion

#region Function to send an HTTP POST request and deserialize the response
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
#endregion

#region Function to export products to an Excel file and send it via email
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
#endregion
async Task Main()
{
    bool valid = false;
    while (!valid)
    {
        Messages.PrintWelcomeMessage();
        Console.WriteLine("How many products do you want to crawler? You can give a number or all options");
        var requestedAmount = Console.ReadLine();

        Console.WriteLine("------------------------------------------------");
        Console.WriteLine("What type of products do you want to crawler?");
        Console.WriteLine("You can choose one option from three options");
        Console.WriteLine("Please enter: A,B or C options.");
        Console.WriteLine("A= All Products, B= On Sale Products, C= Regular Price Products");
        var productCrawlType = Console.ReadLine().ToUpper();

        Console.WriteLine("------------------------------------------------");
        Console.WriteLine("Do you want to receive the crawled products by email?");
        Console.WriteLine("Please enter 'Y' for Yes or 'N' for No:");
        var sendEmailOption = Console.ReadLine().ToUpper();
        bool sendEmail = sendEmailOption == "Y";
        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Welcome to the Crawler Log Page", Guid.Empty));
        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Website logged in", Guid.Empty));
        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Preferences have been received.", Guid.Empty));

        var orderAddRequest = new OrderAddCommand();

        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("The order was successfully received", Guid.Empty));

        bool validAnswer = false;

        while (!validAnswer)
        {
            switch (productCrawlType)
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
                    Console.WriteLine("ERROR: You entered an invalid option!");
                    Thread.Sleep(1500);
                    Console.Clear();
                    break;
            }
        }

        var orderAddResponse = await SendHttpPostRequest<OrderAddCommand, object>(httpClient, ordersUrl, orderAddRequest);
        Guid orderId = orderAddRequest.Id;


        ChromeOptions options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        options.AddArgument("--disable-notifications");
        options.AddArgument("--disable-popup-blocking");

        var Driver = new ChromeDriver(options);
        var Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

        Console.Clear();

        Driver.Navigate().GoToUrl(baseUrl);

        var orderEventAddRequest = new OrderEventAddCommand()
        {
            OrderId = orderId,
            Status = OrderStatus.BotStarted,
        };

        var orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, ordersEventsUrl, orderEventAddRequest);

        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog(OrderStatus.BotStarted.ToString(), Guid.Empty));

        IWebElement pageCountElement = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".pagination > li:nth-last-child(2) > a")));
        int pageCount = int.Parse(pageCountElement.Text);

        Console.WriteLine($"{pageCount} number of pages available.");
        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"{pageCount} number of pages available.", Guid.Empty));
        Console.WriteLine("---------------------------------------");

        int itemCount = 0;

        orderEventAddRequest = new OrderEventAddCommand()
        {
            OrderId = orderId,
            Status = OrderStatus.CrawlingStarted,
        };

        orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, ordersEventsUrl, orderEventAddRequest);

        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog(OrderStatus.CrawlingStarted.ToString(), Guid.Empty));

        for (int p = 1; p <= pageCount; p++)
        {
            Driver.Navigate().GoToUrl($"https://4teker.net/?currentPage={p}");

            Console.WriteLine($"Scanning page {p}...");
            Console.WriteLine($"{p}. Page");

            await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"{p}.page was crawled.", Guid.Empty));

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

            Thread.Sleep(500);

            IReadOnlyCollection<IWebElement> productElements = Driver.FindElements(By.CssSelector(".card.h-100"));

            await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"{productElements.Count} products found.", Guid.Empty));

            foreach (IWebElement productElement in productElements)
            {
                bool includeProduct = false;

                if (productCrawlType == "A" || productCrawlType == "B" || productCrawlType == "C")
                {
                    includeProduct = true;
                }

                if (includeProduct)
                {
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

                        var productAddResponse = await SendHttpPostRequest<ProductAddCommand, object>(httpClient, productsUrl, productAddRequest);

                        productsList.Add(product);

                        await hubConnection.InvokeAsync("SendProductLogNotificationAsync", CreateLog($"Product Name : {productName}" + "   |    " +
                            $"Is On Sale ? :   {isOnSale}" + "   |    " +
                            $"Product Price :   {price}" + "   |    " +
                            $"Product Sale Price :   {salePrice}", Guid.Empty));

                        itemCount++;
                    }
                }

                var orderUpdateRequest = new OrderUpdateCommand()
                {
                    Id = orderId,
                    TotalFoundAmount = itemCount,
                    RequestedAmount = requestedAmount,
                };

                var orderUpdateResponse = await SendHttpPostRequest<OrderUpdateCommand, object>(httpClient, ordersUpdateUrl, orderUpdateRequest);
            }
        }

        Driver.Dispose();

        Console.WriteLine($"{itemCount} products found.");
        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"{itemCount} products of the requested type were found", Guid.Empty));

        orderEventAddRequest = new OrderEventAddCommand()
        {
            OrderId = orderId,
            Status = OrderStatus.CrawlingCompleted,
        };

        orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, ordersEventsUrl, orderEventAddRequest);

        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog(OrderStatus.CrawlingCompleted.ToString(), Guid.Empty));

        orderEventAddRequest = new OrderEventAddCommand()
        {
            OrderId = orderId,
            Status = OrderStatus.OrderCompleted,
        };

        orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, ordersEventsUrl, orderEventAddRequest);

        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog(OrderStatus.OrderCompleted.ToString(), Guid.Empty));

        if (sendEmail)
        {
            Console.WriteLine("Please enter your email address to receive the results:");
            string recipientEmail = Console.ReadLine();

            // ... E-posta gönderme işlemini gerçekleştir ...
            ExportToExcel(productsList, recipientEmail);
        }
        else
        {
            Console.WriteLine("The scraped products will not be sent via email.");
        }
        #region Continue Product Scraping
        Console.WriteLine("Do you want to continue product crawler? ");
        Console.WriteLine("Please enter 'Y' for Yes or 'N' for No:");
        string answerContinueOption = Console.ReadLine().ToUpper();
        bool answerContinue = answerContinueOption == "N";
        if (answerContinue)
        {
            valid = true;
            httpClient.Dispose();

            await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Data scraping completed. Bot Stopped!", Guid.Empty));
            await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Mission is completed!", Guid.Empty));
        }
        else
        {
            await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Bot Will Restart. Website logged in!", Guid.Empty));
        }
        #endregion
    }
}

// Run the Main method asynchronously
Main().GetAwaiter().GetResult();
