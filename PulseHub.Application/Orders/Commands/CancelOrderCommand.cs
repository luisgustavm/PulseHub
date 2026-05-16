// PulseHub.Application/Orders/Commands/CancelOrderCommand.cs

using PulseHub.Application.Common;
using PulseHub.Application.Orders.DTOs;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Application.Orders.Commands;

public record CancelOrderCommand(Guid OrderId, string Reason);

public class CancelOrderHandler
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderDto>> HandleAsync(
        CancelOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Reason))
            return Result<OrderDto>.Failure("Motivo do cancelamento é obrigatório.");

        var order = await _orderRepository.GetByIdWithItemsAsync(command.OrderId, cancellationToken);
        if (order is null)
            return Result<OrderDto>.Failure("Pedido não encontrado.");

        order.Cancel(command.Reason);
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return Result<OrderDto>.Success(order.ToDto());
    }
}
