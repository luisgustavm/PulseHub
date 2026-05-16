// PulseHub.Application/Orders/DTOs/OrderDto.cs

namespace PulseHub.Application.Orders.DTOs;

public record OrderDto(
    Guid Id,
    Guid UserId,
    string UserName,
    string Status,
    decimal TotalAmount,
    DateTime CreatedAt,
    DateTime? ConfirmedAt,
    DateTime? CancelledAt,
    string? CancellationReason,
    IEnumerable<OrderItemDto> Items
);
