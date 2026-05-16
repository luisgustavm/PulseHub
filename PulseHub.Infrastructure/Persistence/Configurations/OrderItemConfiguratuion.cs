using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseHub.Domain.Entities;
namespace PulseHub.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(x => x.Id);

        // ── Propriedades ──────────────────────────────────────

        builder.Property(x => x.Id)
            .ValueGeneratedNever(); // Gerado no domínio

        builder.Property(x => x.ProductName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.Discount)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        // Subtotal e TotalDiscount são computados — não persistidos
        builder.Ignore(x => x.Subtotal);
        builder.Ignore(x => x.TotalDiscount);

        // ── Relacionamentos ───────────────────────────────────

        builder.HasOne(x => x.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // Itens somem com o pedido

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict); // Produto não pode ser deletado se estiver em pedidos

        // ── Índices ───────────────────────────────────────────

        builder.HasIndex(x => x.OrderId)
            .HasDatabaseName("IX_OrderItems_OrderId");

        builder.HasIndex(x => new { x.OrderId, x.ProductId })
            .HasDatabaseName("IX_OrderItems_OrderId_ProductId");
    }
}