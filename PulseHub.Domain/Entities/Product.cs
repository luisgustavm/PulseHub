using PulseHub.Domain.Exceptions;

namespace PulseHub.Domain.Entities;

public class Product
{
    // ── Propriedades ─────────────────────────────────────────
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public bool IsActive { get; private set; }
    public Guid CategoryId { get; private set; }
    public string? ImageUrl { get; private set; }
    public int ViewCount { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // ── Navegação ─────────────────────────────────────────────

    public Category Category { get; private set; } = null!;

    // ── Construtor Privado (EF Core) ──────────────────────────

    private Product() { }

    // ── Factory Method ────────────────────────────────────────

    public static Product Create(
        string name,
        string description,
        decimal price,
        int stock,
        Guid categoryId,
        string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome do produto é obrigatório.");

        if (name.Length > 200)
            throw new DomainException("Nome do produto não pode ter mais de 200 caracteres.");

        if (price <= 0)
            throw new DomainException("Preço deve ser maior que zero.");

        if (stock < 0)
            throw new DomainException("Estoque não pode ser negativo.");

        if (categoryId == Guid.Empty)
            throw new DomainException("Categoria é obrigatória.");

        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Description = description?.Trim() ?? string.Empty,
            Price = price,
            Stock = stock,
            CategoryId = categoryId,
            ImageUrl = imageUrl,
            IsActive = true,
            ViewCount = 0,
            CreatedAt = DateTime.UtcNow
        };
    }

    // ── Comportamentos de Domínio ─────────────────────────────

    public void Update(string name, string description, decimal price, int stock, string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome do produto é obrigatório.");

        if (price <= 0)
            throw new DomainException("Preço deve ser maior que zero.");

        if (stock < 0)
            throw new DomainException("Estoque não pode ser negativo.");

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Price = price;
        Stock = stock;
        ImageUrl = imageUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantidade a adicionar deve ser maior que zero.");

        Stock += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void DeductStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantidade a deduzir deve ser maior que zero.");

        if (quantity > Stock)
            throw new DomainException($"Estoque insuficiente. Disponível: {Stock}. Solicitado: {quantity}.");

        Stock -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool HasStock(int quantity) => Stock >= quantity;

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RegisterView()
    {
        ViewCount++;
        // Não atualiza UpdatedAt — view não é uma edição do produto
    }

    public void ChangeCategory(Guid categoryId)
    {
        if (categoryId == Guid.Empty)
            throw new DomainException("Categoria é obrigatória.");

        CategoryId = categoryId;
        UpdatedAt = DateTime.UtcNow;
    }
}