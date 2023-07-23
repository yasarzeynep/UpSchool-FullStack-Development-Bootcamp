using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations.Application
{
    public class OrderEventConfiguration : IEntityTypeConfiguration<OrderEvent>
    {
        public void Configure(EntityTypeBuilder<OrderEvent> builder)
        {
            // Table name
            builder.ToTable("OrderEvents");
            builder.HasKey(oe => oe.Id);

            // Other column configurations
            // enum //C# enum;  Db enum tipini tanımadığı için, veritabanına int yaz, okurken int->enum çevir
            builder.Property(oe => oe.Status).IsRequired();
            builder.Property(oe => oe.Status).HasConversion<int>();

            // Common Fields

            builder.Property(x => x.CreatedOn).IsRequired();

            // DeletedOn
            builder.Property(x => x.DeletedOn).IsRequired(false);

            //DeletedByUserId
            //builder.Property(x => x.DeletedByUserId).IsRequired(false);
            //builder.Property(x => x.DeletedByUserId).HasMaxLength(100);

            //IsDeleted
            //builder.Property(x => x.IsDeleted).IsRequired();
            //builder.Property(x => x.IsDeleted).HasDefaultValueSql("0");
            //builder.HasIndex(x => x.IsDeleted);

            // Relationships
            builder.HasOne(oe => oe.Order)
                .WithMany(o => o.OrderEvents)
                .HasForeignKey(oe => oe.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

