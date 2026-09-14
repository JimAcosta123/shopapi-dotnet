using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.DTOs;
using ShopApi.Models;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;

    public OrdersController(AppDbContext db)
    {
        _db = db;
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);

    private static OrderResponse ToResponse(Order o) => new(
        o.Id, o.CreatedAt, o.Status.ToString(), o.Total,
        o.Items.Select(i => new OrderItemResponse(i.ProductId, i.Product?.Name ?? "", i.Quantity, i.UnitPrice)).ToList()
    );

    [HttpGet("mine")]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetMyOrders()
    {
        var orders = await _db.Orders
            .Where(o => o.UserId == CurrentUserId)
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return Ok(orders.Select(ToResponse));
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create(CreateOrderRequest request)
    {
        var order = new Order { UserId = CurrentUserId, Status = OrderStatus.Pendiente };
        decimal total = 0;

        foreach (var itemRequest in request.Items)
        {
            var product = await _db.Products.FindAsync(itemRequest.ProductId);
            if (product is null)
                return BadRequest(new { message = $"Producto {itemRequest.ProductId} no existe" });

            if (product.Stock < itemRequest.Quantity)
                return BadRequest(new { message = $"Stock insuficiente para '{product.Name}'" });

            product.Stock -= itemRequest.Quantity;

            var item = new OrderItem
            {
                ProductId = product.Id,
                Quantity = itemRequest.Quantity,
                UnitPrice = product.Price,
            };
            total += item.UnitPrice * item.Quantity;
            order.Items.Add(item);
        }

        order.Total = total;
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        await _db.Entry(order).Collection(o => o.Items).Query().Include(i => i.Product).LoadAsync();

        return CreatedAtAction(nameof(GetMyOrders), ToResponse(order));
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<OrderResponse>> UpdateStatus(int id, [FromQuery] OrderStatus status)
    {
        var order = await _db.Orders
            .Include(o => o.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == CurrentUserId);

        if (order is null) return NotFound(new { message = $"Pedido {id} no encontrado" });

        order.Status = status;
        await _db.SaveChangesAsync();
        return Ok(ToResponse(order));
    }
}
