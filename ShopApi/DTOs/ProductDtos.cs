using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs;

public record ProductRequest(
    [Required] string Name,
    string? Description,
    [Range(0.01, double.MaxValue)] decimal Price,
    [Range(0, int.MaxValue)] int Stock,
    int CategoryId
);

public record ProductResponse(
    int Id, string Name, string? Description, decimal Price, int Stock,
    int CategoryId, string CategoryName
);
