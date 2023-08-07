
using Application.Features.OrderEvents.Commands.Add;
using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Commands.Update;
using Application.Features.Products.Commands.Add;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.VisualBasic;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CrawlerWorkerService
{
    public class Worker : BackgroundService
    {
        #region Constant
        private readonly ILogger<Worker> _logger;
        private readonly string _seleniumLog = "https://localhost:7245/Hubs/SeleniumLogHub";
        private readonly string _orderHub = "https://localhost:7245/Hubs/OrderHub";
        private HubConnection seleniumLogHubConnection;
        private HubConnection orderHubConnection;
        public string RequestedAmountUser { get; set; }
        public string ProductCrawlTypeUser { get; set; }
        private bool crawlerData;
        #endregion
        public Worker(ILogger<Worker> logger)
        {
            seleniumLogHubConnection = new HubConnectionBuilder()
                .WithUrl(_seleniumLog)
                .WithAutomaticReconnect()
                .Build();

            orderHubConnection = new HubConnectionBuilder()
                .WithUrl(_orderHub)
                .WithAutomaticReconnect()
                .Build();

            _logger = logger;
            crawlerData = false;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {


                await Task.WhenAll(
                seleniumLogHubConnection.StartAsync(stoppingToken),
                orderHubConnection.StartAsync(stoppingToken)
            );

                if (seleniumLogHubConnection.State == HubConnectionState.Connected && orderHubConnection.State == HubConnectionState.Connected)
                {
                    seleniumLogHubConnection.On<string>("SendLogNotificationAsync", async (log) => { await Crawler(); });

                    orderHubConnection.On<string, string>("UserReceived", async (requestedAmount, productCrawlType) =>
                    {
                        RequestedAmountUser = requestedAmount;
                        ProductCrawlTypeUser = productCrawlType;



            catch (Exception e)
            {
                Console.WriteLine("Hata: " + e.Message);
            }
        }


        public async Task Crawler()
        {
            crawlerData = true;

            var requestedAmount = RequestedAmountUser;
            var productCrawlType = ProductCrawlTypeUser;


            await seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Welcome to the Crawler Log Page", Guid.Empty));
            await seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Website logged in", Guid.Empty));
            await seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Preferences have been received.", Guid.Empty));

            var orderAddRequest = new OrderAddCommand();

            await seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("The order was successfully received", Guid.Empty));

            bool validAnswer = false;
            #region answer
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
            #endregion

            var orderAddResponse = await SendHttpPostRequest<OrderAddCommand, object>(httpClient, ordersUrl, orderAddRequest);
            Guid orderId = orderAddRequest.Id;

            #region ChromeOptions
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-popup-blocking");

            var Driver = new ChromeDriver(options);
            var Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            #endregion


            Driver.Navigate().GoToUrl(baseUrl);

            var orderEventAddRequest = new OrderEventAddCommand()
            {
                OrderId = orderId,
                Status = OrderStatus.BotStarted,
            };

            var orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, ordersEventsUrl, orderEventAddRequest);

            await seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog(OrderStatus.BotStarted.ToString(), Guid.Empty));

            IWebElement pageCountElement = Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".pagination > li:nth-last-child(2) > a")));
            int pageCount = int.Parse(pageCountElement.Text);

            Console.WriteLine($"{pageCount} number of pages available.");
            await seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"{pageCount} number of pages available.", Guid.Empty));
            Console.WriteLine("---------------------------------------");

            int itemCount = 0;

            orderEventAddRequest = new OrderEventAddCommand()
            {
                OrderId = orderId,
                Status = OrderStatus.CrawlingStarted,
            };

            orderEventAddResponse = await SendHttpPostRequest<OrderEventAddCommand, object>(httpClient, ordersEventsUrl, orderEventAddRequest);

            await seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog(OrderStatus.CrawlingStarted.ToString(), Guid.Empty));

            for (int p = 1; p <= pageCount; p++)
            {
                Driver.Navigate().GoToUrl($"https://4teker.net/?currentPage={p}");

                Console.WriteLine($"Scanning page {p}...");
                Console.WriteLine($"{p}. Page");

                await seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"{p}.page was crawled.", Guid.Empty));

                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                Thread.Sleep(500);
                IReadOnlyCollection<IWebElement> productElements = Driver.FindElements(By.CssSelector(".card.h-100"));

                await seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"{productElements.Count} products found.", Guid.Empty));
                #region product 
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
            #endregion
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

            await seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog(OrderStatus.OrderCompleted.ToString(), Guid.Empty));


            #region Continue Product Scraping
            string answerContinueOption = Console.ReadLine().ToUpper();
            bool answerContinue = answerContinueOption == "N";
            if (answerContinue)
            {
                valid = true;
                httpClient.Dispose();

                await seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Data scraping completed. Bot Stopped!", Guid.Empty));
                await seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Mission is completed!", Guid.Empty));
            }
            else
            {
                await seleniumLogHubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog("Bot Will Restart. Website logged in!", Guid.Empty));

                #endregion
            }
        }
    }
