// PulseHub.Infrastructure/Persistence/Configurations/PaymentConfiguration.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseHub.Domain.Entities;

namespace PulseHub.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        // Enums guardados como int no banco
        // Mais performático que string e ocupa menos espaço
        builder.Property(p => p.Method)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<int>();

        // Valor monetário — NUNCA float
        builder.Property(p => p.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.TransactionCode)
            .HasMaxLength(200);

        builder.Property(p => p.RefusalReason)
            .HasMaxLength(500);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(p => p.PaidAt)
            .HasColumnType("datetime2");

        builder.Property(p => p.RefundedAt)
            .HasColumnType("datetime2");

        // Relacionamento 1:1 — um Order tem no máximo um Payment
        builder.HasOne(p => p.Order)
            .WithOne()
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices para consultas frequentes
        builder.HasIndex(p => p.OrderId)
            .IsUnique() // garante 1 payment por order no banco
            .HasDatabaseName("IX_Payments_OrderId_Unique");

        builder.HasIndex(p => p.Status)
            .HasDatabaseName("IX_Payments_Status");

        builder.HasIndex(p => p.CreatedAt)
            .HasDatabaseName("IX_Payments_CreatedAt");
    }
}