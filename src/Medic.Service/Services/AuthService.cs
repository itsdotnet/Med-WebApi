using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Medic.DataAccess.Repositories;
using Medic.Domain.Entities;
using Medic.Service.DTOs.Users;
using Medic.Service.Exceptions;
using Medic.Service.Helpers;
using Medic.Service.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Medic.Service.Services;

public class AuthService : IAuthService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IConfiguration configuration;
    private readonly IRepository<User> userRepository;
    private readonly IUserService _userService;
    
    public AuthService(IRepository<User> userRepository, IMemoryCache memoryCache, IConfiguration configuration, IUserService userService)
    {
        _memoryCache = memoryCache;
        
        this.configuration = configuration;
        _userService = userService;
        this.userRepository = userRepository;
    }
    
    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await this.userRepository.SelectAsync(u => u.Email.Equals(email));
        if (user is null)
            throw new NotFoundException("This user is not found");

        bool verifiedPassword = password.Verify(user.Password);
        if (!verifiedPassword)
            throw new CustomException(400, "Password is invalid");

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("Email", user.Email),
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        string result = tokenHandler.WriteToken(token);

        _memoryCache.Set(user.Id.ToString(), result, TimeSpan.FromDays(1));

        return result;
    }

    public async Task<string> RegisterAsync(UserCreationDto dto)
    {
        var user = await _userService.CreateAsync(dto);
    
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("Email", user.Email),
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        string result = tokenHandler.WriteToken(token);

        _memoryCache.Set(user.Id.ToString(), result, TimeSpan.FromDays(1));

        return result;
    }
}