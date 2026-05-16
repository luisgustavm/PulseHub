using Microsoft.EntityFrameworkCore;
using PulseHub.Domain.Interfaces;
using PulseHub.Domain.Entities;
using PulseHub.Infrastructure.Persistence;

namespace PulseHub.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly PulseHubDbContext _context;
    public ProductRepository(PulseHubDbContext context)
    {
        _context = context;
    }

    // ── GET POR ID ────────────────────────────────────────────

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Product?> GetByIdWithCategoryAsync(Guid id, CancellationToken ct = default)
        => await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    // ── GET ALL ───────────────────────────────────────────────

    public async Task<IReadOnlyList<Product>> GetAllActiveAsync(CancellationToken ct = default)
        => await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
        => await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .ToListAsync(ct);

    // ── GET POR CATEGORIA ─────────────────────────────────────

    public async Task<IReadOnlyList<Product>> GetByCategoryAsync(
        Guid categoryId, CancellationToken ct = default)
        => await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(ct);

    // ── SEARCH ────────────────────────────────────────────────

    public async Task<IReadOnlyList<Product>> SearchByNameAsync(
        string term, CancellationToken ct = default)
        => await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .Where(p => p.IsActive && EF.Functions.Like(p.Name, $"%{term}%"))
            .OrderBy(p => p.Name)
            .ToListAsync(ct);

    // ── PAGINADO ──────────────────────────────────────────────

    public async Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Guid? categoryId = null,
        bool? isActive = null,
        string? searchTerm = null,
        CancellationToken ct = default)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .AsQueryable();

        // Filtros
        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{searchTerm}%"));

        // Total antes da paginação
        var total = await query.CountAsync(ct);

        // Paginação
        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    // ── TOP VIEWED ────────────────────────────────────────────

    public async Task<IReadOnlyList<Product>> GetTopViewedAsync(
        int count, CancellationToken ct = default)
        => await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.ViewCount)
            .Take(count)
            .ToListAsync(ct);

    // ── LOW STOCK ─────────────────────────────────────────────

    public async Task<IReadOnlyList<Product>> GetLowStockAsync(
        int threshold, CancellationToken ct = default)
        => await _context.Products
            .AsNoTracking()
            .Where(p => p.IsActive && p.Stock <= threshold)
            .OrderBy(p => p.Stock)
            .ToListAsync(ct);

    // ── EXISTS ────────────────────────────────────────────────

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        => await _context.Products.AnyAsync(p => p.Id == id, ct);

    public async Task<bool> NameExistsAsync(
        string name, Guid? excludeId = null, CancellationToken ct = default)
    {
        var query = _context.Products
            .Where(p => p.Name == name);

        if (excludeId.HasValue)
            query = query.Where(p => p.Id != excludeId.Value);

        return await query.AnyAsync(ct);
    }

    // ── ESCRITA ───────────────────────────────────────────────

    public async Task AddAsync(Product product, CancellationToken ct = default)
        => await _context.Products.AddAsync(product, ct);

    public void Update(Product product)
        => _context.Products.Update(product);

    public void Remove(Product product)
        => _context.Products.Remove(product);

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}