using Medic.Domain.Entities;

namespace Medic.Service.Interfaces;

public interface ITokenService
{
    public string GenerateToken(User user);
}
