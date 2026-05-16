// PulseHub.Application/Products/DTOs/ProductMappingExtensions.cs

using PulseHub.Domain.Entities;

namespace PulseHub.Application.Products.DTOs;

public static class ProductMappingExtensions
{
    public static ProductDto ToDto(this Product product) =>
        new(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.IsActive,
            product.CategoryId,
            product.Category?.Name ?? string.Empty,
            product.ImageUrl,
            product.ViewCount,
            product.CreatedAt);
}
