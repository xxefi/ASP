using ApiFirst.Data.Models;
using System.Security.Claims;

namespace ApiFirst.Services.Interfaces;

public interface ITokenService
{
    public Task<string> GenerateTokenAsync(User user);
    public Task<string> GenerateRefreshTokenAsync();
    public ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifeTime = false);
    public Task<string> GenerateEmailTokenAsync(string userId);
}
