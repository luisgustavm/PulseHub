// PulseHub.Application/Categories/Commands/CreateCategoryCommand.cs

using PulseHub.Application.Categories.DTOs;
using PulseHub.Application.Common;
using PulseHub.Domain.Entities;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Application.Categories.Commands;

public record CreateCategoryCommand(string Name, string Description);

public class CreateCategoryHandler
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryDto>> HandleAsync(
        CreateCategoryCommand command,
        CancellationToken cancellationToken = default)
    {
        if (await _categoryRepository.ExistsByNameAsync(command.Name, cancellationToken))
            return Result<CategoryDto>.Failure("Já existe uma categoria com este nome.");

        var category = Category.Create(command.Name, command.Description);

        await _categoryRepository.AddAsync(category, cancellationToken);

        return Result<CategoryDto>.Success(category.ToDto());
    }
}
