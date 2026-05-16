using PulseHub.Domain.Entities;

namespace PulseHub.Domain.Interfaces;

public interface IProductRepository
{
    // ── Leitura ───────────────────────────────────────────────
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Product?> GetByIdWithCategoryAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyList<Product>> GetAllActiveAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default);

    Task<IReadOnlyList<Product>> GetByCategoryAsync(Guid categoryId, CancellationToken ct = default);

    /// <summary>
    /// Busca produtos pelo nome (LIKE — case insensitive).
    /// </summary>
    Task<IReadOnlyList<Product>> SearchByNameAsync(string term, CancellationToken ct = default);

    /// <summary>
    /// Retorna produtos paginados, com filtro opcional por categoria e status.
    /// </summary>
    Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Guid? categoryId = null,
        bool? isActive = null,
        string? searchTerm = null,
        CancellationToken ct = default);

    /// <summary>
    /// Retorna os N produtos mais vistos (para cache trending).
    /// </summary>
    Task<IReadOnlyList<Product>> GetTopViewedAsync(int count, CancellationToken ct = default);

    /// <summary>
    /// Retorna produtos com estoque abaixo do threshold.
    /// </summary>
    Task<IReadOnlyList<Product>> GetLowStockAsync(int threshold, CancellationToken ct = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
    Task<bool> NameExistsAsync(string name, Guid? excludeId = null, CancellationToken ct = default);

    // ── Escrita ───────────────────────────────────────────────

    Task AddAsync(Product product, CancellationToken ct = default);
    void Update(Product product);
    void Remove(Product product);
    Task SaveChangesAsync(CancellationToken ct = default);
}