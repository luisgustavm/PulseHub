// PulseHub.Application/Orders/Queries/GetOrderByIdHandler.cs

using PulseHub.Application.Common;
using PulseHub.Application.Orders.DTOs;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Application.Orders.Queries;

public record GetOrderByIdQuery(Guid OrderId);

public class GetOrderByIdHandler
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderDto>> HandleAsync(
        GetOrderByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(query.OrderId, cancellationToken);
        if (order is null)
            return Result<OrderDto>.Failure("Pedido não encontrado.");

        return Result<OrderDto>.Success(order.ToDto());
    }
}
