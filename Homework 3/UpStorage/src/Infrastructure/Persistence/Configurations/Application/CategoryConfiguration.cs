﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Application;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        //Id
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();

        //Name
        builder.Property(x => x.Name).IsRequired();
        builder.Property(X => X.Name).HasMaxLength(200);
        builder.HasIndex(x => x.Name);

        // Common Fields

        // CreatedOn
        builder.Property(x => x.CreatedOn).IsRequired();

        // CreatedByUserId
        builder.Property(x => x.CreatedByUserId).IsRequired(false);
        builder.Property(x => x.CreatedByUserId).HasMaxLength(100);

        // ModifiedOn
        builder.Property(x => x.ModifiedOn).IsRequired(false);

        // ModifiedByUserId
        builder.Property(x => x.ModifiedByUserId).IsRequired(false);
        builder.Property(x => x.ModifiedByUserId).HasMaxLength(100);

        // DeletedOn
        builder.Property(x => x.DeletedOn).IsRequired(false);

        // DeletedByUserId
        builder.Property(x => x.DeletedByUserId).IsRequired(false);
        builder.Property(x => x.DeletedByUserId).HasMaxLength(100);

        // IsDeleted
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.IsDeleted).HasDefaultValueSql("0");
        builder.HasIndex(x => x.IsDeleted);

        builder.ToTable("Categories");
    }
}
