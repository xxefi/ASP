using ApiFirst.Data.Contexts;
using ApiFirst.Data.Models;
using ApiFirst.Data.Models.DTOS;
using ApiFirst.Exceptions;
using ApiFirst.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static BCrypt.Net.BCrypt;

namespace ApiFirst.Services.Classes;

public class AuthService : IAuthService
{
    private readonly AuthContext _context;
    private readonly ITokenService _tokenService;
    private readonly IBlackListService _blackListService;
    private readonly IEmailSender _emailSender;

    public AuthService(AuthContext context, ITokenService tokenService, IBlackListService blackListService, IEmailSender emailSender)
    {
        _context = context;
        _tokenService = tokenService;
        _blackListService = blackListService;
        _emailSender = emailSender;
    }

    public async Task<AccessInfoDTO> LoginUserAsync(LoginDTO user)
    {
        try
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username)
                ??
                throw new MyAuthException(AuthErrorTypes.UserNotFound, "User not found");

            if (Verify(user.Password, foundUser.Password))
            {
                var tokenData = new AccessInfoDTO
                {
                    AccessToken = await _tokenService.GenerateTokenAsync(foundUser),
                    RefreshToken = await _tokenService.GenerateRefreshTokenAsync(),
                    RefreshTokenExpireTime = DateTime.UtcNow.AddDays(1)
                };
                foundUser.RefreshToken = tokenData.RefreshToken;
                foundUser.RefreshTokenExpiryTime = tokenData.RefreshTokenExpireTime;

                await _context.SaveChangesAsync();
                return tokenData;
            }
            else
                throw new MyAuthException(AuthErrorTypes.InvalidCredentials, "Invalid Credentials");

        }
        catch { throw; }

    }

    public async Task LogOutAsync(TokenDTO userTokenInfo)
    {
        if (userTokenInfo is null)
            throw new MyAuthException(AuthErrorTypes.InvalidRequest, "Invalid client request");

        var principal = _tokenService.GetPrincipalFromToken(userTokenInfo.AccessToken);

        var username = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = DateTime.Now;
        await _context.SaveChangesAsync();

        _blackListService.AddTokenToBlackList(userTokenInfo.AccessToken);
    }

    public async Task<AccessInfoDTO> RefreshTokenAsync(TokenDTO userAccessData)
    {
        if (userAccessData is null)
            throw new MyAuthException(AuthErrorTypes.InvalidRequest, "Invalid client request");

        var accessToken = userAccessData.AccessToken;
        var refreshToken = userAccessData.RefreshToken;

        var principal = _tokenService.GetPrincipalFromToken(accessToken);

        var username = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new MyAuthException(AuthErrorTypes.InvalidRequest, "Invalid client request");

        var newAccessToken = await _tokenService.GenerateTokenAsync(user);
        var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);

        await _context.SaveChangesAsync();

        return new AccessInfoDTO
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            RefreshTokenExpireTime = user.RefreshTokenExpiryTime,
        };

    }

    public async Task<User> RegisterUserAsync(RegisterDTO user)
    {
        try
        {
            var newUser = new User
            {
                Username = user.Username,
                Email = user.Email,
                Password = HashPassword(user.Password),
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }
        catch { throw; }
    }
}
