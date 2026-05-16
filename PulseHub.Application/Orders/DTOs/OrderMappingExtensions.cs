// PulseHub.Application/Orders/DTOs/OrderMappingExtensions.cs

using PulseHub.Domain.Entities;

namespace PulseHub.Application.Orders.DTOs;

public static class OrderMappingExtensions
{
    public static OrderDto ToDto(this Order order) =>
        new(
            order.Id,
            order.UserId,
            order.User?.Name ?? string.Empty,
            order.Status.ToString(),
            order.TotalAmount,
            order.CreatedAt,
            order.ConfirmedAt,
            order.CancelledAt,
            order.CancellationReason,
            order.Items.Select(i => i.ToDto()));

    public static OrderItemDto ToDto(this OrderItem item) =>
        new(
            item.Id,
            item.ProductId,
            item.ProductName,
            item.UnitPrice,
            item.Quantity,
            item.Discount,
            item.Subtotal);

    public static OrderSummaryDto ToSummaryDto(this Order order) =>
        new(
            order.Id,
            order.UserId,
            order.Status.ToString(),
            order.TotalAmount,
            order.Items.Count,
            order.CreatedAt);
}
