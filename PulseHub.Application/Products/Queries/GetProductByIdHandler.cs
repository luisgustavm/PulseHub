// PulseHub.Application/Products/Queries/GetProductByIdHandler.cs

using PulseHub.Application.Common;
using PulseHub.Application.Products.DTOs;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Application.Products.Queries;

public record GetProductByIdQuery(Guid ProductId);

public class GetProductByIdHandler
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductDto>> HandleAsync(
        GetProductByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdWithCategoryAsync(query.ProductId, cancellationToken);
        if (product is null)
            return Result<ProductDto>.Failure("Produto não encontrado.");

        return Result<ProductDto>.Success(product.ToDto());
    }
}
