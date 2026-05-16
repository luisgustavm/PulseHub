// PulseHub.Application/Users/Queries/GetUsersHandler.cs

using PulseHub.Application.Common;
using PulseHub.Application.Users.DTOs;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Application.Users.Queries;

// ─────────────────────────────────────────────────────────────
// QUERY — representa a intenção de buscar usuários.
// Records são imutáveis e perfeitos para queries de leitura.
// Parâmetros opcionais permitem filtros flexíveis.
// ─────────────────────────────────────────────────────────────
public record GetUsersQuery(
    string? SearchTerm = null,   // filtro por nome ou email
    bool? IsActive = null,   // null = todos | true = ativos | false = inativos
    int Page = 1,
    int PageSize = 20
);

// ─────────────────────────────────────────────────────────────
// HANDLER — executa a query e retorna os dados paginados.
// Não tem lógica de negócio: apenas orquestra a consulta.
// ─────────────────────────────────────────────────────────────
public class GetUsersHandler
{
    private readonly IUserRepository _repository;

    public GetUsersHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    // ─────────────────────────────────────────────────────────
    // HandleAsync — ponto de entrada do handler.
    //
    // Fluxo:
    //   1. Valida os parâmetros de paginação
    //   2. Delega a consulta ao repositório
    //   3. Mapeia entidades → DTOs
    //   4. Retorna resultado paginado encapsulado em Result<T>
    // ─────────────────────────────────────────────────────────
    public async Task<Result<PagedResult<UserDto>>> HandleAsync(
        GetUsersQuery query,
        CancellationToken cancellationToken = default)
    {
        // ── Validação dos parâmetros de paginação ─────────────
        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize < 1 ? 20 : query.PageSize;
        pageSize = pageSize > 100 ? 100 : pageSize; // limite máximo

        // ── Busca paginada no repositório ─────────────────────
        var (users, total) = await _repository.GetPagedAsync(
            searchTerm: query.SearchTerm,
            isActive: query.IsActive,
            page: page,
            pageSize: pageSize,
            cancellationToken);

        // ── Mapeamento: Domain Entity → DTO ───────────────────
        // NUNCA exponha a entidade de domínio diretamente na API.
        // O DTO é um contrato de saída — controla o que o cliente vê.
        var dtos = users.Select(u => new UserDto(
            Id: u.Id,
            Name: u.Name,
            Email: u.Email,
            IsActive: u.IsActive,
            CreatedAt: u.CreatedAt));

        // ── Monta resultado paginado ──────────────────────────
        var pagedResult = new PagedResult<UserDto>(
            Items: dtos,
            Total: total,
            Page: page,
            PageSize: pageSize);

        return Result<PagedResult<UserDto>>.Success(pagedResult);
    }
}