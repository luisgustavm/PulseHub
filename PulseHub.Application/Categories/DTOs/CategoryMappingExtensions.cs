// PulseHub.Application/Categories/DTOs/CategoryMappingExtensions.cs

using PulseHub.Domain.Entities;

namespace PulseHub.Application.Categories.DTOs;

public static class CategoryMappingExtensions
{
    public static CategoryDto ToDto(this Category category) =>
        new(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedAt);
}
