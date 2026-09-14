using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs;

public record OrderItemRequest(
    [Required] int ProductId,
    [Range(1, int.MaxValue)] int Quantity
);

public record CreateOrderRequest(
    [Required, MinLength(1)] List<OrderItemRequest> Items
);

public record OrderItemResponse(int ProductId, string ProductName, int Quantity, decimal UnitPrice);

public record OrderResponse(int Id, DateTime CreatedAt, string Status, decimal Total, List<OrderItemResponse> Items);
