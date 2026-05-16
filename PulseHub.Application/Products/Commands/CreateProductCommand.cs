// PulseHub.Application/Products/Commands/CreateProductCommand.cs

using PulseHub.Application.Common;
using PulseHub.Application.Products.DTOs;
using PulseHub.Domain.Entities;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Application.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId,
    string? ImageUrl = null
);

public class CreateProductHandler
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<ProductDto>> HandleAsync(
        CreateProductCommand command,
        CancellationToken cancellationToken = default)
    {
        if (!await _categoryRepository.ExistsAsync(command.CategoryId, cancellationToken))
            return Result<ProductDto>.Failure("Categoria não encontrada.");

        if (await _productRepository.NameExistsAsync(command.Name, null, cancellationToken))
            return Result<ProductDto>.Failure("Já existe um produto com este nome.");

        var product = Product.Create(
            command.Name,
            command.Description,
            command.Price,
            command.Stock,
            command.CategoryId,
            command.ImageUrl);

        await _productRepository.AddAsync(product, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);

        var saved = await _productRepository.GetByIdWithCategoryAsync(product.Id, cancellationToken);

        return Result<ProductDto>.Success(saved!.ToDto());
    }
}
