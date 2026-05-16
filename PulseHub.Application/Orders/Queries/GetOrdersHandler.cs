// PulseHub.Application/Orders/Queries/GetOrdersHandler.cs

using PulseHub.Application.Common;
using PulseHub.Application.Orders.DTOs;
using PulseHub.Domain.Entities;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Application.Orders.Queries;

public record GetOrdersQuery(
    int Page = 1,
    int PageSize = 20,
    Guid? UserId = null,
    OrderStatus? Status = null,
    DateTime? From = null,
    DateTime? To = null
);

public class GetOrdersHandler
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<PagedResult<OrderSummaryDto>>> HandleAsync(
        GetOrdersQuery query,
        CancellationToken cancellationToken = default)
    {
        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize < 1 ? 20 : Math.Min(query.PageSize, 100);

        var (orders, total) = await _orderRepository.GetPagedAsync(
            page, pageSize,
            query.UserId, query.Status,
            query.From, query.To,
            cancellationToken);

        var dtos = orders.Select(o => o.ToSummaryDto());

        return Result<PagedResult<OrderSummaryDto>>.Success(
            new PagedResult<OrderSummaryDto>(dtos, total, page, pageSize));
    }
}
