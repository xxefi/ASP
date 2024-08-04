namespace ApiFirst.Data.Models.DTOS;

public class AccessInfoDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpireTime { get; set; }
}
