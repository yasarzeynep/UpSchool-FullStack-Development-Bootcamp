using System.Reflection;
using Domain.Entities;
using Domain.Identity;
using Domain.Settings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Contexts;

public class IdentityContext:IdentityDbContext<User,Role,string,UserClaim,UserRole,UserLogin,RoleClaim,UserToken>
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Ignores
        modelBuilder.Ignore<Product>();
        modelBuilder.Ignore<Order>();
        modelBuilder.Ignore<OrderEvent>();
       

        base.OnModelCreating(modelBuilder);
    }
}