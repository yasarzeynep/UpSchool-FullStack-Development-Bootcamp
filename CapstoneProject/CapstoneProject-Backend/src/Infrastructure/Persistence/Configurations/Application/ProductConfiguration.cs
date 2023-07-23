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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Table name
            builder.ToTable("Products");
            builder.HasKey(p => p.Id);


            // Other column configurations
            // enum //C# enum;  Db enum tipini tanımadığı için, veritabanına int yaz, okurken int->enum çevir
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.Picture).HasMaxLength(200);
            builder.Property(p => p.IsOnSale).IsRequired();
            builder.Property(p => p.SalePrice).HasColumnType("decimal(18,2)").IsRequired();

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
            builder.HasOne(p => p.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

