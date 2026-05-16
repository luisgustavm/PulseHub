// PulseHub.API/Controllers/CategoriesController.cs

using Microsoft.AspNetCore.Mvc;
using PulseHub.Application.Categories.Commands;
using PulseHub.Application.Categories.DTOs;
using PulseHub.Application.Categories.Queries;

namespace PulseHub.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly CreateCategoryHandler _createHandler;
    private readonly GetCategoriesHandler _getCategoriesHandler;

    public CategoriesController(
        CreateCategoryHandler createHandler,
        GetCategoriesHandler getCategoriesHandler)
    {
        _createHandler = createHandler;
        _getCategoriesHandler = getCategoriesHandler;
    }

    /// <summary>Lista todas as categorias</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool onlyActive = true,
        CancellationToken cancellationToken = default)
    {
        var result = await _getCategoriesHandler.HandleAsync(
            new GetCategoriesQuery(onlyActive), cancellationToken);

        return Ok(result.Value);
    }

    /// <summary>Cria uma nova categoria</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _createHandler.HandleAsync(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return StatusCode(StatusCodes.Status201Created, result.Value);
    }
}
