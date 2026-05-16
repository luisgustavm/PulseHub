// PulseHub.Application/Common/PagedResult.cs

namespace PulseHub.Application.Common;

// ─────────────────────────────────────────────────────────────
// PagedResult<T> — wrapper genérico para qualquer listagem
// paginada da aplicação.
//
// Por que record?
//   Records são imutáveis por padrão. Um resultado de consulta
//   nunca deve ser modificado após ser criado — record é perfeito.
//
// Uso:
//   var result = new PagedResult<UserDto>(dtos, total, page, pageSize);
//   Console.WriteLine(result.TotalPages); // 25
// ─────────────────────────────────────────────────────────────
public record PagedResult<T>(
    IEnumerable<T> Items,
    int Total,
    int Page,
    int PageSize)
{
    // Calculado automaticamente — não precisa ser passado
    public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPrevPage => Page > 1;
}