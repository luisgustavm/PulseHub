// PulseHub.Application/Categories/DTOs/CategoryDto.cs

namespace PulseHub.Application.Categories.DTOs;

public record CategoryDto(
    Guid Id,
    string Name,
    string Description,
    bool IsActive,
    DateTime CreatedAt
);
