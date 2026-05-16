// PulseHub.Application/Orders/DTOs/OrderSummaryDto.cs

namespace PulseHub.Application.Orders.DTOs;

public record OrderSummaryDto(
    Guid Id,
    Guid UserId,
    string Status,
    decimal TotalAmount,
    int ItemCount,
    DateTime CreatedAt
);
