using ApiFirst.Data.Models;
using ApiFirst.Services.Interfaces;
using System.Security.Claims;

namespace ApiFirst.Services.Classes;

public class TokenService : ITokenService
{
    public Task<string> GenerateEmailTokenAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<string> GenerateRefreshTokenAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> GenerateTokenAsync(User user)
    {
        throw new NotImplementedException();
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifeTime = false)
    {
        throw new NotImplementedException();
    }
}
