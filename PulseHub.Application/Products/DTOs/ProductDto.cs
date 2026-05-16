// PulseHub.Application/Products/DTOs/ProductDto.cs

namespace PulseHub.Application.Products.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    bool IsActive,
    Guid CategoryId,
    string CategoryName,
    string? ImageUrl,
    int ViewCount,
    DateTime CreatedAt
);
