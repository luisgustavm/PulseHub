// PulseHub.Infrastructure/Persistence/Repositories/PaymentRepository.cs

using Microsoft.EntityFrameworkCore;
using PulseHub.Domain.Entities;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Infrastructure.Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PulseHubDbContext _context;

    public PaymentRepository(PulseHubDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Payment?> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        // Sem AsNoTracking — pode precisar atualizar o status
        return await _context.Payments
            .Include(p => p.Order)
            .FirstOrDefaultAsync(p => p.OrderId == orderId, cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetByStatusAsync(
        PaymentStatus status,
        CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .AsNoTracking()
            .Where(p => p.Status == status)
            .Include(p => p.Order)
            .OrderBy(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasApprovedPaymentAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        // Protege contra cobrança duplicada —
        // verifica se o pedido já tem um pagamento aprovado
        return await _context.Payments
            .AnyAsync(
                p => p.OrderId == orderId &&
                     p.Status == PaymentStatus.Approved,
                cancellationToken);
    }

    public async Task AddAsync(
        Payment payment,
        CancellationToken cancellationToken = default)
    {
        await _context.Payments.AddAsync(payment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Update(Payment payment)
    {
        _context.Payments.Update(payment);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}