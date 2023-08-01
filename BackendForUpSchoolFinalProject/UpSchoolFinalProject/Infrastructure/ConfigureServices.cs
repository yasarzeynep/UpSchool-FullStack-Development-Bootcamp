using Application.Common.Interfaces;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration, string wwwrootPath)
        {
            //var connectionString = configuration.GetConnectionString("MariaDB");

            //services.AddDbContext<ApplicationDbContext>(opt => opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Scoped Services : Bir request geldiğinde ihtiyac duydukça aynı referansı kullanmak
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());


            services.AddScoped<IExcelService, ExcelManager>();

            // Singleton Services
            services.AddSingleton<IEmailService>(new EmailManager(wwwrootPath));

            return services;
        }
    }
}
