// PulseHub.Domain/Entities/Address.cs

using PulseHub.Domain.Exceptions;

namespace PulseHub.Domain.Entities;

/// <summary>
/// Value Object representando um endereço de entrega ou cobrança.
/// Imutável após criação — use o factory method Create.
/// </summary>
public class Address
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public string Street { get; private set; } = string.Empty;
    public string Number { get; private set; } = string.Empty;
    public string? Complement { get; private set; }
    public string Neighborhood { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string ZipCode { get; private set; } = string.Empty;
    public bool IsDefault { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Address() { }

    public static Address Create(
        Guid userId,
        string street,
        string number,
        string neighborhood,
        string city,
        string state,
        string zipCode,
        string? complement = null,
        bool isDefault = false)
    {
        if (userId == Guid.Empty)
            throw new DomainException("UserId inválido.");

        ArgumentException.ThrowIfNullOrWhiteSpace(street, nameof(street));
        ArgumentException.ThrowIfNullOrWhiteSpace(number, nameof(number));
        ArgumentException.ThrowIfNullOrWhiteSpace(neighborhood, nameof(neighborhood));
        ArgumentException.ThrowIfNullOrWhiteSpace(city, nameof(city));
        ArgumentException.ThrowIfNullOrWhiteSpace(state, nameof(state));
        ArgumentException.ThrowIfNullOrWhiteSpace(zipCode, nameof(zipCode));

        return new Address
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Street = street.Trim(),
            Number = number.Trim(),
            Complement = complement?.Trim(),
            Neighborhood = neighborhood.Trim(),
            City = city.Trim(),
            State = state.Trim().ToUpperInvariant(),
            ZipCode = zipCode.Trim(),
            IsDefault = isDefault,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void SetAsDefault() => IsDefault = true;
    public void UnsetDefault() => IsDefault = false;
}
