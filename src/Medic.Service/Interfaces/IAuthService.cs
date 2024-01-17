using Medic.Service.DTOs.Users;

namespace Medic.Service.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(string email, string password);
    Task<string> RegisterAsync(UserCreationDto dto);
}