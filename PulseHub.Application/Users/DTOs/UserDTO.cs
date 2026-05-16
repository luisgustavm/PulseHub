// PulseHub.Application/Users/DTOs/UserDto.cs

namespace PulseHub.Application.Users.DTOs;

///<summary>
/// DTO (Data Transfer Object) — o que o cliente recebe.
/// Nunca exponha sua entidade de domínio diretamente na API.
/// O DTO é um contrato de saída.
///</summary>
public record UserDto(
    Guid Id,
    string Name,
    string Email,
    bool IsActive,
    DateTime CreatedAt
);