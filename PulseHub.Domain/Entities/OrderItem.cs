using PulseHub.Domain.Exceptions;

namespace PulseHub.Domain.Entities;

public class OrderItem
{
    // ── Propriedades ─────────────────────────────────────────
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; }   // snapshot do nome no momento do pedido
    public decimal UnitPrice { get; private set; }    // snapshot do preço no momento do pedido
    public int Quantity { get; private set; }
    public decimal Discount { get; private set; }     // valor absoluto do desconto por item

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // ── Navegação ─────────────────────────────────────────────

    public Order Order { get; private set; } = null!;
    public Product Product { get; private set; } = null!;

    // ── Propriedades Calculadas ───────────────────────────────

    /// <summary>
    /// Subtotal do item: (UnitPrice - Discount) * Quantity
    /// </summary>
    public decimal Subtotal => (UnitPrice - Discount) * Quantity;

    /// <summary>
    /// Total de desconto aplicado ao item: Discount * Quantity
    /// </summary>
    public decimal TotalDiscount => Discount * Quantity;

    // ── Construtor Privado (EF Core) ──────────────────────────

    private OrderItem() { }

    // ── Factory Method ────────────────────────────────────────

    /// <summary>
    /// Cria um novo item de pedido com validações de domínio.
    /// Usa snapshot de nome e preço para manter histórico fiel.
    /// </summary>
    public static OrderItem Create(
        Guid orderId,
        Guid productId,
        string productName,
        decimal unitPrice,
        int quantity,
        decimal discount = 0)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("OrderId é obrigatório.");

        if (productId == Guid.Empty)
            throw new DomainException("ProductId é obrigatório.");

        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("Nome do produto é obrigatório.");

        if (unitPrice <= 0)
            throw new DomainException("Preço unitário deve ser maior que zero.");

        if (quantity <= 0)
            throw new DomainException("Quantidade deve ser maior que zero.");

        if (discount < 0)
            throw new DomainException("Desconto não pode ser negativo.");

        if (discount >= unitPrice)
            throw new DomainException("Desconto não pode ser maior ou igual ao preço unitário.");

        return new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            ProductId = productId,
            ProductName = productName,
            UnitPrice = unitPrice,
            Quantity = quantity,
            Discount = discount,
            CreatedAt = DateTime.UtcNow
        };
    }

    // ── Comportamentos de Domínio ─────────────────────────────

    /// <summary>
    /// Atualiza a quantidade do item. Mínimo: 1.
    /// </summary>
    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new DomainException("Quantidade deve ser maior que zero.");

        Quantity = newQuantity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Aplica ou atualiza o desconto por unidade.
    /// </summary>
    public void ApplyDiscount(decimal discount)
    {
        if (discount < 0)
            throw new DomainException("Desconto não pode ser negativo.");

        if (discount >= UnitPrice)
            throw new DomainException("Desconto não pode ser maior ou igual ao preço unitário.");

        Discount = discount;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Remove o desconto aplicado ao item.
    /// </summary>
    public void RemoveDiscount()
    {
        Discount = 0;
        UpdatedAt = DateTime.UtcNow;
    }
}