using Medic.Service.DTOs.Users;

namespace Medic.Service.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(string email, string password);
    Task<string> RegisterAsync(UserCreationDto dto);
    // public Task<(bool Result, int CashedMinutes)> RegisterAsync(UserCreationDto registerDto);
    // public Task<(bool Result, int CashedVerificationMinutes)> SendCodeForRegisterAsync(string mail);
    // public Task<(bool Result, string Token)> VerifyRegisterAsync(string mail, int code);
    // public Task<(bool Result, string Token)> LoginAsync(string mail, string password);
}