// PulseHub.Domain/Entities/User.cs

using PulseHub.Domain.Exceptions;
using System.Net;

namespace PulseHub.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    // Navegação — EF Core usa isso para os JOINs
    private readonly List<Order> _orders = new();
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    private readonly List<Address> _addresses = new();
    public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

    // EF Core precisa de construtor sem parâmetros (privado está ok)
    private User() { }

    public static User Create(string name, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));

        if (!email.Contains('@'))
            throw new DomainException("Formato de email inválido.");

        return new User
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Email = email.ToLowerInvariant().Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        Name = name.Trim();
    }

    public void Deactivate()
    {
        if (!IsActive) throw new DomainException("Usuário já está inativo.");
        IsActive = false;
        DeletedAt = DateTime.UtcNow;
    }

    public void Reactivate()
    {
        if (IsActive) throw new DomainException("Usuário já está ativo.");
        IsActive = true;
        DeletedAt = null;
    }
}