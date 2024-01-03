using Medic.Service.DTOs.Users;

namespace Medic.Service.Interfaces;

public interface IAuthService
{
    Task<string> Login(string email, string password);
    Task<string> Register(UserCreationDto dto);
}