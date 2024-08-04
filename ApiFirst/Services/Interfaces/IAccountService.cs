using ApiFirst.Data.Models.DTOS;

namespace ApiFirst.Services.Interfaces;

public interface IAccountService
{
    public Task ResetPasswordAsync(ResetPasswordDTO resetRequest, string token);
    public Task ConfirmEmailAsync(string token);
}
