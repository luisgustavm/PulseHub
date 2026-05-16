// PulseHub.Application/Users/Commands/CreateUserCommand.cs

using PulseHub.Application.Common;
using PulseHub.Application.Users.DTOs;
using PulseHub.Domain.Entities;
using PulseHub.Domain.Interfaces;

namespace PulseHub.Application.Users.Commands;

///<summary>
/// Command — representa a intenção de criar um usuário.
/// Records são imutáveis — perfeito para commands.
///</summary>
public record CreateUserCommand(
    string Name,
    string Email
);

///<summary>
/// Handler — executa o command.
/// Lógica de orquestração: valida, cria, persiste, retorna.
///</summary>
public class CreateUserHandler
{
    private readonly IUserRepository _repository;

    public CreateUserHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<UserDto>> HandleAsync(
        CreateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Verificar se email já existe
        var emailExists = await _repository.ExistsByEmailAsync(
            command.Email, cancellationToken);

        if (emailExists)
            return Result<UserDto>.Failure("Email já cadastrado");

        // 2. Criar entidade via factory method (validação automática)
        var user = User.Create(command.Name, command.Email);

        // 3. Persistir
        await _repository.AddAsync(user, cancellationToken);

        // 4. Retornar DTO (nunca a entidade diretamente)
        var dto = new UserDto(
            user.Id,
            user.Name,
            user.Email,
            user.IsActive,
            user.CreatedAt);

        return Result<UserDto>.Success(dto);
    }
}