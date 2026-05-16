// PulseHub.API/Controllers/UsersController.cs

using Microsoft.AspNetCore.Mvc;
using PulseHub.Application.Common;
using PulseHub.Application.Users.Commands;
using PulseHub.Application.Users.DTOs;
using PulseHub.Application.Users.Queries;

namespace PulseHub.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly CreateUserHandler _createHandler;
    private readonly GetUsersHandler _getUsersHandler;

    public UsersController(
        CreateUserHandler createHandler,
        GetUsersHandler getUsersHandler)
    {
        _createHandler = createHandler;
        _getUsersHandler = getUsersHandler;
    }

    ///<summary>
    /// Lista todos os usuários com suporte a filtro e paginação
    ///</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? searchTerm,
        [FromQuery] bool? isActive,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUsersQuery(searchTerm, isActive, page, pageSize);
        var result = await _getUsersHandler.HandleAsync(query, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    ///<summary>
    /// Cria um novo usuário
    ///</summary>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _createHandler.HandleAsync(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return CreatedAtAction(
            nameof(GetAll),
            new { id = result.Value!.Id },
            result.Value);
    }
}