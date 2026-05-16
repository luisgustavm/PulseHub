// PulseHub.Application/Orders/DTOs/OrderItemDto.cs

namespace PulseHub.Application.Orders.DTOs;

public record OrderItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal Discount,
    decimal Subtotal
);
