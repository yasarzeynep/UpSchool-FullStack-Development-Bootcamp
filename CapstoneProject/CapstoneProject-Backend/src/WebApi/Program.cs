using Application;
using Application.Common.Interfaces;
using Domain.Settings;
using ExcelDataReader.Log;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi.Filters;
using WebApi.Hubs;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserManager>();

// Add services to the container.

builder.Services.AddControllers(opt =>
    {
        //opt.Filters.Add<ValidationFilter>();
        opt.Filters.Add<GlobalExceptionFilter>();
    });

    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

    builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection("GoogleSettings"));

    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(setupAction =>
    {
    setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = $"Input your Bearer token in this format - Bearer token to access this API",
    });
        setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            }, new List<string>()
        },
    });
    });
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
        o.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/Hubs/LogHub") || (path.StartsWithSegments("/Hubs/OrderHub"))))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });



    builder.Services.AddSignalR();

    //builder.Services.AddScoped<IAccountHubService, AccountHubManager>();

    builder.Services.AddMemoryCache();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            builder => builder
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed((host) => true)
                .AllowAnyHeader());
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseStaticFiles();

//Localization
//var requestLocalizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
//if (requestLocalizationOptions is not null) app.UseRequestLocalization(requestLocalizationOptions.Value);

app.UseHttpsRedirection();

    app.UseCors("AllowAll");

    app.UseAuthentication();

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







