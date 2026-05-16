// PulseHub.Domain/Interfaces/ICategoryRepository.cs

using PulseHub.Domain.Entities;

namespace PulseHub.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Category?> GetByIdWithProductsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
    void Update(Category category);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
