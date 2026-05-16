// PulseHub.Infrastructure/Persistence/PulseHubDbContext.cs

using Microsoft.EntityFrameworkCore;
using PulseHub.Domain.Entities;

namespace PulseHub.Infrastructure.Persistence;

public class PulseHubDbContext : DbContext
{
    public PulseHubDbContext(DbContextOptions<PulseHubDbContext> options)
        : base(options) { }

    // DbSets — uma propriedade por entidade principal
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Address> Addresses => Set<Address>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica todas as configurações de entities automaticamente
        // Busca todos os tipos que implementam IEntityTypeConfiguration<>
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(PulseHubDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}