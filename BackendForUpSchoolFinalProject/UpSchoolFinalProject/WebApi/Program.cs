using Application;
using Infrastructure;
using Infrastructure.Persistence.Contexts;
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

var mariaDbConnectionString = builder.Configuration.GetConnectionString("MariaDB")!;
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseMySql(mariaDbConnectionString, ServerVersion.AutoDetect(mariaDbConnectionString)));

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

app.UseRouting();


app.UseAuthorization();

app.MapControllers();

/******Hub******
*Hub'lar, SignalR uygulamalarının temel yapı taşlarıdır.
*Bir hub, gelen istemci bağlantılarını işler,
*istemci gruplarını yönetir
*Ve istemcilerin çağırabileceği yöntemleri sunar.*/

//app.MapHub<AccountsHub>("/Hubs/AccountsHub");  // Bağlanacağımız adres
app.MapHub<SeleniumLogHub>("/Hubs/SeleniumLogHub");  // Erişim sagladığımız url'li verdiğimiz yer

app.Run();
