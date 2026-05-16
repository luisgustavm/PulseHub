// PulseHub.Application/Orders/Commands/ConfirmOrderCommand.cs

using PulseHub.Application.Common;
using PulseHub.Application.Orders.DTOs;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Application.Orders.Commands;

public record ConfirmOrderCommand(Guid OrderId);

public class ConfirmOrderHandler
{
    private readonly IOrderRepository _orderRepository;

    public ConfirmOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderDto>> HandleAsync(
        ConfirmOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(command.OrderId, cancellationToken);
        if (order is null)
            return Result<OrderDto>.Failure("Pedido não encontrado.");

        order.Confirm();
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return Result<OrderDto>.Success(order.ToDto());
    }
}
