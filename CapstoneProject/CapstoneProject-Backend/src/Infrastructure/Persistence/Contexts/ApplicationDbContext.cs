using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Configurations.Application;
using Infrastructure.Persistence.Seeder;
using Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderEvent> OrderEvents { get; set; }

        //appsetting connettion stringden gelen bilgiyi kullanıyor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurations; ApplicationDbContext teki tüm konfgürasyonları okuyacak
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderEventConfiguration());


            // Seeds
            modelBuilder.ApplyConfiguration(new ProductSeeder());
            modelBuilder.ApplyConfiguration(new OrderSeeder());

            // Ignores 

            base.OnModelCreating(modelBuilder);
        }
    }
}
