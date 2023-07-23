using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations.Application
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Table name
            builder.ToTable("Orders");
            builder.HasKey(x => x.Id);

            
            builder.Property(o => o.RequestedAmount).IsRequired();
            builder.Property(o => o.TotalFoundAmount).IsRequired();

            // enum //C# enum;  Db enum tipini tanımadığı için, veritabanına int yaz, okurken int->enum çevir
            builder.Property(o => o.ProductCrawlType).IsRequired();
            builder.Property(o => o.ProductCrawlType).HasConversion<int>(); 

            // Common Fields

            

            // DeletedOn
            builder.Property(x => x.DeletedOn).IsRequired(false);

            // DeletedByUserId
            builder.Property(x => x.DeletedByUserId).IsRequired(false);
            builder.Property(x => x.DeletedByUserId).HasMaxLength(100);

            // IsDeleted
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.IsDeleted).HasDefaultValueSql("0");
            builder.HasIndex(x => x.IsDeleted);

            // Relationships
            builder.HasMany(o => o.OrderEvents)
                .WithOne(oe => oe.Order)
                .HasForeignKey(oe => oe.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.Products)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
