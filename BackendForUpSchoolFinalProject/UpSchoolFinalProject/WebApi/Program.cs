using Application;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

//var mariaDbConnectionString = builder.Configuration.GetConnectionString("MariaDB")!;
//builder.Services.AddDbContext<UpStorageDbContext>(opt => opt.UseMySql(mariaDbConnectionString, ServerVersion.AutoDetect(mariaDbConnectionString)));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader());
});


// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.WebRootPath);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseStaticFiles();

//// Localization
//var requestLocalizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
//if (requestLocalizationOptions is not null) app.UseRequestLocalization(requestLocalizationOptions.Value);


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

/******Hub******
*Hub'lar, SignalR uygulamalarýnýn temel yapý taþlarýdýr.
*Bir hub, gelen istemci baðlantýlarýný iþler,
*istemci gruplarýný yönetir
*Ve istemcilerin çaðýrabileceði yöntemleri sunar.*/

//app.MapHub<AccountsHub>("/Hubs/AccountsHub");  // Baðlanacaðýmýz adres
app.MapHub<SeleniumLogHub>("/Hubs/SeleniumLogHub");  // Eriþim sagladýðýmýz url'li verdiðimiz yer

app.Run();
