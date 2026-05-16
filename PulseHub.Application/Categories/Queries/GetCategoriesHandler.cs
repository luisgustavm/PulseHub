// PulseHub.Application/Categories/Queries/GetCategoriesHandler.cs

using PulseHub.Application.Categories.DTOs;
using PulseHub.Application.Common;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Application.Categories.Queries;

public record GetCategoriesQuery(bool OnlyActive = true);

public class GetCategoriesHandler
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<IEnumerable<CategoryDto>>> HandleAsync(
        GetCategoriesQuery query,
        CancellationToken cancellationToken = default)
    {
        var categories = query.OnlyActive
            ? await _categoryRepository.GetAllActiveAsync(cancellationToken)
            : await _categoryRepository.GetAllAsync(cancellationToken);

        var dtos = categories.Select(c => c.ToDto());

        return Result<IEnumerable<CategoryDto>>.Success(dtos);
    }
}
