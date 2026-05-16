using PulseHub.Domain.Entities;

namespace PulseHub.Domain.Interfaces;

public interface IOrderRepository
{
    // ── Leitura ───────────────────────────────────────────────
    /// <summary>
    /// Retorna o pedido com itens e usuário incluídos.
    /// Usado quando precisa manipular o pedido (tracking ativo).
    /// </summary>
    Task<Order?> GetByIdWithItemsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna pedido simples por Id, sem includes.
    /// Usado para validações rápidas e operações de status.
    /// </summary>
    Task<Order?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna todos os pedidos de um usuário, ordenados por data decrescente.
    /// Inclui os itens de cada pedido (AsNoTracking).
    /// </summary>
    Task<IEnumerable<Order>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna pedidos filtrados por status, ordenados por data crescente.
    /// Útil para filas de processamento e painéis operacionais.
    /// </summary>
    Task<IEnumerable<Order>> GetByStatusAsync(
        OrderStatus status,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna pedidos paginados com filtros opcionais.
    /// </summary>
    Task<(IEnumerable<Order> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Guid? userId = null,
        OrderStatus? status = null,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se um pedido pertence ao usuário informado.
    /// Usado para autorização antes de operações sensíveis.
    /// </summary>
    Task<bool> BelongsToUserAsync(
        Guid orderId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    // ── Escrita ───────────────────────────────────────────────

    /// <summary>
    /// Persiste um novo pedido e salva no banco.
    /// </summary>
    Task AddAsync(
        Order order,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza um pedido existente e salva no banco.
    /// </summary>
    Task UpdateAsync(
        Order order,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Salva alterações pendentes no contexto.
    /// Usar quando múltiplas operações precisam ser commitadas juntas.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}