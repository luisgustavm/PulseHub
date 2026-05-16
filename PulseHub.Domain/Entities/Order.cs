// PulseHub.Domain/Entities/Order.cs

using PulseHub.Domain.Exceptions;

namespace PulseHub.Domain.Entities;

public enum OrderStatus
{
    Pending = 1,
    Confirmed = 2,
    Processing = 3,
    Shipped = 4,
    Delivered = 5,
    Cancelled = 6
}

public class Order
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public string? CancellationReason { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { }

    public static Order Create(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new DomainException("UserId inválido.");

        return new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Status = OrderStatus.Pending,
            TotalAmount = 0,
            CreatedAt = DateTime.UtcNow
        };
    }

    // Adiciona item e recalcula o total automaticamente
    public void AddItem(Product product, int quantity, decimal discount = 0)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Não é possível adicionar itens a um pedido confirmado.");

        var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id);

        if (existingItem is not null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var item = OrderItem.Create(Id, product.Id, product.Name, product.Price, quantity, discount);
            _items.Add(item);
        }

        RecalculateTotal();
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Apenas pedidos pendentes podem ser confirmados.");

        if (!_items.Any())
            throw new DomainException("Pedido sem itens não pode ser confirmado.");

        Status = OrderStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
    }

    public void Cancel(string reason)
    {
        if (Status is OrderStatus.Delivered or OrderStatus.Cancelled)
            throw new DomainException("Pedido não pode ser cancelado.");

        Status = OrderStatus.Cancelled;
        CancellationReason = reason;
        CancelledAt = DateTime.UtcNow;
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Sum(i => i.Subtotal);
    }
}