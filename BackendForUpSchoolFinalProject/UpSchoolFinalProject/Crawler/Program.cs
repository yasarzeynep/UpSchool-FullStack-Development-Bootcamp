
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;
using OpenQA.Selenium.Chrome;
using Domain.Entities;
using AngleSharp.Dom;
using OpenQA.Selenium;
using Crawler;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Domain.Enums;
using Application.Common.Dtos;
using Microsoft.AspNetCore.SignalR.Client;


Thread.Sleep(5000);
Console.WriteLine("Please, press any key");
Console.ReadKey();
//---------------------------------------------------------------
var hubConnection = new HubConnectionBuilder()
                .WithUrl($"https://localhost:7285/Hubs/SeleniumLogHub") //Wasm>wwwroot>appsetting.json  
                .WithAutomaticReconnect()
                .Build();

await hubConnection.StartAsync();
//----------------------------------------------------------------

try

{
    Messages.PrintWelcomeMessage();
    DateTime now = DateTime.Now;
    Console.WriteLine("Website logged in. - " + now.ToString("dd.MM.yyyy : HH:mm"));
    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Bot started." + now.ToString("dd.MM.yyyy : HH: mm")));
    //---------
    //var httpClient = new HttpClient();

    //var apiSendNotificationDto = new SendLogNotificationApiDto(CreateLog("Bot started."), hubConnection.ConnectionId);

    //await httpClient.PostAsJsonAsync("https://localhost:7296/api/SeleniumLogs", apiSendNotificationDto);

    new DriverManager().SetUpDriver(new ChromeConfig());

    IWebDriver driver = new ChromeDriver();

    driver.Navigate().GoToUrl("https://finalproject.dotnet.gg/");
    List<Product> products = new List<Product>();
    string sample = @"(\d+)";


    Console.WriteLine(DateTimeOffset.Now + " Website login.");

    ReadOnlyCollection<IWebElement> pagecount = driver.FindElements(By.CssSelector(".page-item"));

    Console.WriteLine(DateTimeOffset.Now + " Total number of pages = " + (pagecount.Count - 1));
    //Product detail ; cardbody

    // IReadOnlyCollection<IWebElement> cardBody = driver.FindElements(By.CssSelector(".card.h-100"));

    Console.WriteLine("------------------------------------------------");
    Console.WriteLine("What products do you want to scrape?");
    Console.WriteLine("You can choose one option from three options");
    Console.WriteLine("1= All Products, 2= On Sale Products, 3= Regular Price Products");
   
    //int counter = 0;
    int answer = 0;
    bool validAnswer = false;

    while (!validAnswer)
    {
        Messages.ScrapingQuestion();
        string input = Console.ReadLine();

        if (int.TryParse(input, out answer))
        {
            int counter = 0;
            switch (answer)
            {
                case 1:

                    Console.WriteLine("All Products:");
                    for (int i = 0; i < products.Count; i++)
                    {
                        Product product = products[i];
                        Console.WriteLine($"Product {i + 1} Details");
                        PrintProductDetails(product);
                        Console.WriteLine("------------------------");
                    }
                    validAnswer = true;

                    break;
                case 2:
                    Console.WriteLine("On Sale Products:");
                    Console.WriteLine("On Sale Products:");
                    int counter1 = 1;
                    foreach (Product product in products.Where(p => p.IsOnSale))
                    {
                        Console.WriteLine($"Product {counter1} Details");
                        PrintProductDetails(product);
                        Console.WriteLine("------------------------");
                        counter1++;
                    }
                    validAnswer = true;
                    break;
                case 3:
                    Console.WriteLine("Regular Price Products:");
                    int counter2 = 1;
                    foreach (Product product in products.Where(p => !p.IsOnSale))
                    {
                        Console.WriteLine($"Product {counter2} Details");
                        PrintProductDetails(product);
                        Console.WriteLine("------------------------");
                        counter2++;
                    }
                    validAnswer = true;
                    break;
                default:
                    Console.WriteLine("ERROR:You entered an invalid option.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Please enter: 1, 2 or 3 options.");
        }
    }

    for (int i = 0; i < pagecount.Count; i++)
    {
        driver.Navigate().GoToUrl("https://finalproject.dotnet.gg/");
        IReadOnlyCollection<IWebElement> productElements = driver.FindElements(By.CssSelector(".card"));
        foreach (IWebElement productElement in productElements)
        {
            string name = productElement.FindElement(By.CssSelector(".product-name")).Text;
            string price = productElement.FindElement(By.CssSelector(".price")).Text;
            string picture = productElement.FindElement(By.CssSelector(".card-img-top")).GetAttribute("src");

            price = price.Replace("$", "");

            Match match = Regex.Match(picture, sample);

            bool isOnSale = productElement.FindElements(By.CssSelector(".onsale")).Count > 0;

            if (isOnSale == true)
            {
                string onSalePrice = productElement.FindElement(By.CssSelector(".sale-price")).Text;
                onSalePrice = onSalePrice.Replace("$", "");

                products.Add(new Product { Name = name, Price = decimal.Parse(price), Picture = picture, IsOnSale = isOnSale, SalePrice = decimal.Parse(onSalePrice), OrderId = Guid.NewGuid() });

            }

            else
            {
                products.Add(new Product { Name = name, Price = decimal.Parse(price), Picture = picture, IsOnSale = isOnSale, OrderId = Guid.NewGuid() });

            }


        }
        Console.WriteLine(($"Page {products.Count} scanned. Total {products.Count} products - " + now.ToString("dd.MM.yyyy : HH:mm")));
        await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Page {pagecount} scanned. Total {products.Count} products - " + now.ToString("dd.MM.yyyy : HH:mm")));
        Console.WriteLine(($"Page {pagecount} scanned. Total {products.Count} products - " + now.ToString("dd.MM.yyyy : HH:mm")));

    }
    var totalProduct = products.Count;
    Console.WriteLine(($"{totalProduct} products detected - " + now.ToString("dd.MM.yyyy : HH:mm")));
    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"{totalProduct} products detected - " + now.ToString("dd.MM.yyyy : HH:mm")));
    Console.ReadKey();
    driver.Quit();

    //foreach (Product product in products)
    //{
    //    if (product.IsOnSale)
    //    {
    //        Console.WriteLine(counter + "==>Product Details");
    //        Console.WriteLine($"Name: {product.Name}");
    //        Console.WriteLine($"Price: {product.Price}");
    //        Console.WriteLine($"Image: {product.Picture}");
    //        Console.WriteLine($"Is On Sale: {product.IsOnSale}");
    //        Console.WriteLine($"ID: {product.OrderId}");
    //        Console.WriteLine($"Product On Sale Price: {product.SalePrice}");
    //        Console.WriteLine("------------------------");
    //        counter++;
    //    }
    //}



    Console.WriteLine("Mission Completed. - " + now.ToString("dd.MM.yyyy : HH:mm"));
    
    await hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Data scraping completed. - " + now.ToString("dd.MM.yyyy : HH:mm")));
}



catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
static void PrintProductDetails(Product product)
{
    Console.WriteLine($"Name: {product.Name}");
    Console.WriteLine($"Price: {product.Price}");
    Console.WriteLine($"Image: {product.Picture}");
    Console.WriteLine($"Is On Sale: {product.IsOnSale}");
    Console.WriteLine($"ID: {product.OrderId}");
    if (product.IsOnSale)
    {
        Console.WriteLine($"Product On Sale Price: {product.SalePrice}");
    }
}


SeleniumLogDto CreateLog(string message) => new SeleniumLogDto(message);




