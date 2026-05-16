// PulseHub.Application/Orders/Commands/CreateOrderCommand.cs

using PulseHub.Application.Common;
using PulseHub.Application.Orders.DTOs;
using PulseHub.Domain.Entities;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Application.Orders.Commands;

public record CreateOrderCommand(Guid UserId);

public class CreateOrderHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;

    public CreateOrderHandler(
        IOrderRepository orderRepository,
        IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<OrderSummaryDto>> HandleAsync(
        CreateOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user is null)
            return Result<OrderSummaryDto>.Failure("Usuário não encontrado.");

        if (!user.IsActive)
            return Result<OrderSummaryDto>.Failure("Usuário inativo não pode realizar pedidos.");

        var order = Order.Create(command.UserId);
        await _orderRepository.AddAsync(order, cancellationToken);

        return Result<OrderSummaryDto>.Success(new OrderSummaryDto(
            order.Id,
            order.UserId,
            order.Status.ToString(),
            order.TotalAmount,
            0,
            order.CreatedAt));
    }
}
