// PulseHub.Application/Orders/Commands/AddOrderItemCommand.cs

using PulseHub.Application.Common;
using PulseHub.Application.Orders.DTOs;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Application.Orders.Commands;

public record AddOrderItemCommand(
    Guid OrderId,
    Guid ProductId,
    int Quantity,
    decimal Discount = 0
);

public class AddOrderItemHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public AddOrderItemHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Result<OrderDto>> HandleAsync(
        AddOrderItemCommand command,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(command.OrderId, cancellationToken);
        if (order is null)
            return Result<OrderDto>.Failure("Pedido não encontrado.");

        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);
        if (product is null)
            return Result<OrderDto>.Failure("Produto não encontrado.");

        if (!product.IsActive)
            return Result<OrderDto>.Failure("Produto indisponível.");

        if (!product.HasStock(command.Quantity))
            return Result<OrderDto>.Failure($"Estoque insuficiente. Disponível: {product.Stock}.");

        order.AddItem(product, command.Quantity, command.Discount);
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return Result<OrderDto>.Success(order.ToDto());
    }
}
