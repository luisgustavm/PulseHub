// PulseHub.Application/Products/Queries/GetProductsHandler.cs

using PulseHub.Application.Common;
using PulseHub.Application.Products.DTOs;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Application.Products.Queries;

public record GetProductsQuery(
    int Page = 1,
    int PageSize = 20,
    Guid? CategoryId = null,
    bool? IsActive = null,
    string? SearchTerm = null
);

public class GetProductsHandler
{
    private readonly IProductRepository _productRepository;

    public GetProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<PagedResult<ProductDto>>> HandleAsync(
        GetProductsQuery query,
        CancellationToken cancellationToken = default)
    {
        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        var (items, total) = await _productRepository.GetPagedAsync(
            page, pageSize,
            query.CategoryId,
            query.IsActive,
            query.SearchTerm,
            cancellationToken);

        var dtos = items.Select(p => p.ToDto());

        return Result<PagedResult<ProductDto>>.Success(
            new PagedResult<ProductDto>(dtos, total, page, pageSize));
    }
}
