using ApiFirst.Data.Contexts;
using ApiFirst.Data.Models.DTOS;
using ApiFirst.Exceptions;
using ApiFirst.Services.Interfaces;
using System.Security.Claims;
using static BCrypt.Net.BCrypt;

namespace ApiFirst.Services.Classes;

public class AccountService : IAccountService
{
    private readonly IEmailSender _emailSender;
    private readonly ITokenService _tokenService;
    private readonly AuthContext _context;

    public AccountService(IEmailSender emailSender, ITokenService tokenService, AuthContext context)
    {
        _emailSender = emailSender;
        _tokenService = tokenService;
        _context = context;
    }

    public async Task ConfirmEmailAsync(string token)
    {
        var principal = _tokenService.GetPrincipalFromToken(token, validateLifeTime: true);

        var username = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var user = _context.Users.FirstOrDefault(u => u.Username == username)
            ??
            throw new MyAuthException(AuthErrorTypes.UserNotFound, "User not found");

        var confirmationToken = await _tokenService.GenerateEmailTokenAsync(user.Id.ToString());

        var link = $"https://localhost:7014/api/v1/Account/ValidateConfirmation?token={confirmationToken}&userId={user.Id}";

        await _emailSender.SendEmailAsync(user.Email, $"Please confirm your account by <a href='{link}'>clicking here</a>;.", link);
    }

    public async Task ResetPasswordAsync(ResetPasswordDTO resetRequest, string token)
    {
        var principal = _tokenService.GetPrincipalFromToken(token, validateLifeTime: true);

        var username = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var user = _context.Users.FirstOrDefault(u => u.Username == username)
            ??
            throw new MyAuthException(AuthErrorTypes.UserNotFound, "User not found");

        if (!Verify(resetRequest.OldPassword, user.Password))
            throw new MyAuthException(AuthErrorTypes.InvalidCredentials, "Invalid Credentials");

        if (resetRequest.NewPassword != resetRequest.ConfirmNewPassword)
            throw new MyAuthException(AuthErrorTypes.PasswordMismatch, "Passwords do not match");

        user.Password = HashPassword(resetRequest.NewPassword);

        await _emailSender.SendEmailAsync(user.Email, "Password Reset", "Your Password has bee reset");
        await _context.SaveChangesAsync();
    }
}
