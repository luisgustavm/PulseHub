// PulseHub.Infrastructure/Persistence/Configurations/UserConfiguration.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseHub.Domain.Entities;

namespace PulseHub.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Nome da tabela
        builder.ToTable("Users");

        // Chave primária
        builder.HasKey(u => u.Id);

        // Configurações de colunas
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("Name");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(254)  // RFC 5321 — tamanho máximo de email
            .HasColumnName("Email");

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(u => u.DeletedAt)
            .HasColumnType("datetime2");

        // Índice único no email — não pode ter dois usuários com mesmo email
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email_Unique");

        // Índice para buscas por usuários ativos
        builder.HasIndex(u => u.IsActive)
            .HasDatabaseName("IX_Users_IsActive");

        // Relacionamento 1:N — User tem muitos Orders
        builder.HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Não deleta Orders ao deletar User
    }
}