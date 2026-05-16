// PulseHub.Domain/Entities/Category.cs

using PulseHub.Domain.Exceptions;

namespace PulseHub.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navegação — uma categoria tem muitos produtos
    private readonly List<Product> _products = new();
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private Category() { }

    public static Category Create(string name, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

        if (name.Length > 200)
            throw new DomainException("Nome da categoria não pode ultrapassar 200 caracteres.");

        return new Category
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Description = description?.Trim() ?? string.Empty,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

        if (name.Length > 200)
            throw new DomainException("Nome da categoria não pode ultrapassar 200 caracteres.");

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException("Categoria já está inativa.");

        if (_products.Any(p => p.IsActive))
            throw new DomainException("Não é possível desativar uma categoria com produtos ativos.");

        IsActive = false;
    }

    public void Reactivate()
    {
        if (IsActive)
            throw new DomainException("Categoria já está ativa.");

        IsActive = true;
    }
}