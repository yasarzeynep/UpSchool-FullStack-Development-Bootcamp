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
*Hub'lar, SignalR uygulamalar�n�n temel yap� ta�lar�d�r.
*Bir hub, gelen istemci ba�lant�lar�n� i�ler,
*istemci gruplar�n� y�netir
*Ve istemcilerin �a��rabilece�i y�ntemleri sunar.*/

//app.MapHub<AccountsHub>("/Hubs/AccountsHub");  // Ba�lanaca��m�z adres
app.MapHub<SeleniumLogHub>("/Hubs/SeleniumLogHub");  // Eri�im saglad���m�z url'li verdi�imiz yer

app.Run();
