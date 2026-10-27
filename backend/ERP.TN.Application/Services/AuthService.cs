using ERP.TN.Application.DTOs;
using ERP.TN.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ERP.TN.Application.Services;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}

public class AuthService : IAuthService
{
    private static readonly Dictionary<string, (string Password, string Role)> Users = new()
    {
        ["admin"] = ("admin123", "Admin"),
        ["finance"] = ("finance123", "Finance"),
        ["achats"] = ("achats123", "Achats"),
        ["ventes"] = ("ventes123", "Ventes"),
        ["logistique"] = ("logistique123", "Logistique")
    };

    public Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        if (!Users.TryGetValue(request.Username, out var credentials))
            return Task.FromResult<LoginResponse?>(null);

        if (!string.Equals(credentials.Password, request.Password, StringComparison.Ordinal))
            return Task.FromResult<LoginResponse?>(null);

        var token = GenerateJwtToken(request.Username, credentials.Role);

        return Task.FromResult<LoginResponse?>(new LoginResponse
        {
            Token = token,
            Username = request.Username,
            Role = credentials.Role
        });
    }

    private static string GenerateJwtToken(string username, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ERP_TUNISIE_SECRET_KEY_FOR_LOCAL_DEVELOPMENT_2026"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "ERP-TN",
            audience: "ERP-TN-Clients",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
