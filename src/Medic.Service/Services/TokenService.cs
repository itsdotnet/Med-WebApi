using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Medic.Domain.Constants;
using Medic.Domain.Entities;
using Medic.Service.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Medic.Service.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    public TokenService(IConfiguration configuration)
    {
        _config = configuration.GetSection("JWT");
    }

    public string GenerateToken(User user)
    {
        var identityClaims = new Claim[]
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("FirstName", user.Firstname),
            new Claim("LastName", user.Lastname),
            new Claim("Role", user.UserRole.ToString()),
            new Claim("Email", user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecurityKey"]!));
        var keyCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        int expiresHours = int.Parse(_config["Lifetime"]!);
        var token = new JwtSecurityToken(
            issuer: _config["Issuer"],
            audience: _config["Audience"],
            claims: identityClaims,
            expires: TimeConstants.GetNow().AddHours(expiresHours),
            signingCredentials: keyCredentials );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}