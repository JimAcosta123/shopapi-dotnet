using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs;

public record RegisterRequest(
    [Required] string Name,
    [Required, EmailAddress] string Email,
    [Required, MinLength(6)] string Password
);

public record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password
);

public record AuthResponse(int Id, string Name, string Email, string Token);
