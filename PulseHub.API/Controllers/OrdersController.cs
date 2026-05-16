// PulseHub.API/Controllers/OrdersController.cs

using Microsoft.AspNetCore.Mvc;
using PulseHub.Application.Common;
using PulseHub.Application.Orders.Commands;
using PulseHub.Application.Orders.DTOs;
using PulseHub.Application.Orders.Queries;
using PulseHub.Domain.Entities;

namespace PulseHub.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly CreateOrderHandler _createOrderHandler;
    private readonly AddOrderItemHandler _addItemHandler;
    private readonly ConfirmOrderHandler _confirmHandler;
    private readonly CancelOrderHandler _cancelHandler;
    private readonly GetOrdersHandler _getOrdersHandler;
    private readonly GetOrderByIdHandler _getByIdHandler;

    public OrdersController(
        CreateOrderHandler createOrderHandler,
        AddOrderItemHandler addItemHandler,
        ConfirmOrderHandler confirmHandler,
        CancelOrderHandler cancelHandler,
        GetOrdersHandler getOrdersHandler,
        GetOrderByIdHandler getByIdHandler)
    {
        _createOrderHandler = createOrderHandler;
        _addItemHandler = addItemHandler;
        _confirmHandler = confirmHandler;
        _cancelHandler = cancelHandler;
        _getOrdersHandler = getOrdersHandler;
        _getByIdHandler = getByIdHandler;
    }

    /// <summary>Lista pedidos paginados com filtros opcionais</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<OrderSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? userId = null,
        [FromQuery] OrderStatus? status = null,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetOrdersQuery(page, pageSize, userId, status, from, to);
        var result = await _getOrdersHandler.HandleAsync(query, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    /// <summary>Retorna um pedido com seus itens</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getByIdHandler.HandleAsync(new GetOrderByIdQuery(id), cancellationToken);

        if (!result.IsSuccess)
            return NotFound(new { error = result.Error });

        return Ok(result.Value);
    }

    /// <summary>Cria um novo pedido para o usuário</summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrderSummaryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _createOrderHandler.HandleAsync(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    /// <summary>Adiciona um item ao pedido</summary>
    [HttpPost("{id:guid}/items")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddItem(
        Guid id,
        [FromBody] AddItemRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddOrderItemCommand(id, request.ProductId, request.Quantity, request.Discount);
        var result = await _addItemHandler.HandleAsync(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    /// <summary>Confirma o pedido</summary>
    [HttpPost("{id:guid}/confirm")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Confirm(Guid id, CancellationToken cancellationToken)
    {
        var result = await _confirmHandler.HandleAsync(new ConfirmOrderCommand(id), cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    /// <summary>Cancela o pedido</summary>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cancel(
        Guid id,
        [FromBody] CancelRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _cancelHandler.HandleAsync(
            new CancelOrderCommand(id, request.Reason), cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }
}

public record AddItemRequest(Guid ProductId, int Quantity, decimal Discount = 0);
public record CancelRequest(string Reason);
