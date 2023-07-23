using CrawlerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<HttpClient>(new HttpClient() { BaseAddress = new Uri("http://localhost:7245/api/") });
    })
    .Build();

host.Run();
