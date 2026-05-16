// PulseHub.Infrastructure/Persistence/Repositories/UserRepository.cs

using Microsoft.EntityFrameworkCore;
using PulseHub.Domain.Entities;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PulseHubDbContext _context;

    public UserRepository(PulseHubDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // AsNoTracking → EF não fica "monitorando" o objeto
        // Perfeito para consultas de leitura — mais rápido
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByIdWithOrdersAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Include → faz JOIN e carrega Orders junto
        // Sem AsNoTracking porque pode precisar salvar mudanças
        return await _context.Users
            .Include(u => u.Orders)
                .ThenInclude(o => o.Items) // JOIN aninhado
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .OrderBy(u => u.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllActiveAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.IsActive)           // Filtro — gera WHERE
            .OrderBy(u => u.Name)             // Ordenação — gera ORDER BY
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<User> Users, int Total)> GetPagedAsync(
        string? searchTerm = null,
        bool? isActive = null,
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Users
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(u =>
                u.Name.Contains(searchTerm) ||
                u.Email.Contains(searchTerm));

        if (isActive.HasValue)
            query = query.Where(u => u.IsActive == isActive.Value);

        query = query.OrderBy(u => u.Name);

        var total = await query.CountAsync(cancellationToken);

        var users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (users, total);
    }

    public async Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        // AnyAsync é mais eficiente que FirstOrDefault para checar existência
        // Gera: SELECT CASE WHEN EXISTS(...) THEN 1 ELSE 0END
        return await _context.Users
            .AnyAsync(u => u.Email == email.ToLowerInvariant(), cancellationToken);
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}