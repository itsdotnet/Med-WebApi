using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Medic.DataAccess.Repositories;
using Medic.DataAccess.UnitOfWorks;
using Medic.Domain.Constants;
using Medic.Domain.Entities;
using Medic.Service.DTOs.Users;
using Medic.Service.Exceptions;
using Medic.Service.Helpers;
using Medic.Service.Interfaces;
using Medic.Service.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Medic.Service.Services;

public class AuthService : IAuthService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMailSender _mailSender;
    private readonly ITokenService _tokenService;
    private const int CACHED_MINUTES_FOR_REGISTER = 60;
    private const int CACHED_MINUTES_FOR_VERIFICATION = 5;
    private const string REGISTER_CACHE_KEY = "register_";
    private const string VERIFY_REGISTER_CACHE_KEY = "verify_register_";
    private const int VERIFICATION_MAXIMUM_ATTEMPTS = 3;

    public AuthService(IMemoryCache memoryCache,
        IUnitOfWork unitOfWork,
        IMailSender mailSender,
        ITokenService tokenService)
    {
        _memoryCache = memoryCache;
        _unitOfWork = unitOfWork;
        _mailSender = mailSender;
        _tokenService = tokenService;
    }

    public async Task<(bool Result, int CashedMinutes)> RegisterAsync(UserCreationDto registerDto)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(x => x.Email.Equals(registerDto.Email.ToLower()));
        if (user is not null) throw new AlreadyExistException("User is already exist with this email");

        if (_memoryCache.TryGetValue(REGISTER_CACHE_KEY + registerDto.Email, out UserCreationDto cachedRegisterDto))
        {
            cachedRegisterDto.Firstname = cachedRegisterDto.Firstname;
            _memoryCache.Remove(REGISTER_CACHE_KEY + registerDto.Email);
        }
        else _memoryCache.Set(REGISTER_CACHE_KEY + registerDto.Email, registerDto,
            TimeSpan.FromMinutes(CACHED_MINUTES_FOR_REGISTER));

        return (Result: true, CachedMinutes: CACHED_MINUTES_FOR_REGISTER);
    }
    
    public async Task<(bool Result, int CashedVerificationMinutes)> SendCodeForRegisterAsync(string mail)
    {
        if (_memoryCache.TryGetValue(REGISTER_CACHE_KEY + mail, out UserCreationDto registerDto))
        {
            UserVerficationDto verificationDto = new UserVerficationDto();
            verificationDto.Attempt = 0;
            verificationDto.CreatedAt = TimeConstants.GetNow();


            verificationDto.Code = CodeGenerator.GenerateRandomNumber();


            if (_memoryCache.TryGetValue(VERIFY_REGISTER_CACHE_KEY + mail, out UserVerficationDto oldDto))
            {
                _memoryCache.Remove(VERIFY_REGISTER_CACHE_KEY + mail);
            }
            _memoryCache.Set(VERIFY_REGISTER_CACHE_KEY + mail, verificationDto,
                TimeSpan.FromMinutes(CACHED_MINUTES_FOR_VERIFICATION));

            EmailMessage emailSms = new EmailMessage();
            emailSms.Title = "Madical UZ";
            emailSms.Content = "Verification code : " + verificationDto.Code;
            emailSms.Recipent = mail;

            var mailResult = await _mailSender.SendAsync(emailSms);
            if (mailResult is true) return (Result: true, CachedVerificationMinutes: CACHED_MINUTES_FOR_VERIFICATION);
            else return (Result: false, CachedVerificationMinutes: 0);
        }
        else throw new CustomException(410, "Registration time expired");
    }

    public async Task<(bool Result, string Token)> VerifyRegisterAsync(string mail, int code)
    {
        if(_memoryCache.TryGetValue(REGISTER_CACHE_KEY + mail, out UserCreationDto dto))
        {
            if(_memoryCache.TryGetValue(VERIFY_REGISTER_CACHE_KEY + mail, out UserVerficationDto verificationDto))
            {
                if (verificationDto.Attempt >= VERIFICATION_MAXIMUM_ATTEMPTS)
                    throw new CustomException(429, "Too many requests");
                else if (verificationDto.Code == code)
                {
                    var dbResult = await RegisterToDatabaseAsync(dto);
                    if(dbResult is true)
                    {
                        var user = await _unitOfWork.UserRepository.SelectAsync(x => x.Email == mail.ToLower());
                        string token = _tokenService.GenerateToken(user);
                        return (Result: true, Token: token);
                    }
                    else return (Result: false, Token: "");
                }
                else
                {
                    _memoryCache.Remove(VERIFY_REGISTER_CACHE_KEY + mail);
                    verificationDto.Attempt++;
                    _memoryCache.Set(VERIFY_REGISTER_CACHE_KEY + mail, verificationDto,
                        TimeSpan.FromMinutes(CACHED_MINUTES_FOR_VERIFICATION));
                    return (Result: false, Token: "");
                }
            }
            else throw new CustomException(410, "Verfication code time expired");
        }
        else throw new CustomException(410, "Registration time expired");
    }
    
    private async Task<bool> RegisterToDatabaseAsync(UserCreationDto registroDto)
    {
        var user = new User();
        user.Firstname = registroDto.Firstname;
        user.Lastname = registroDto.Lastname;
        user.Email = registroDto.Email.ToLower();
        user.DateOfBirth = registroDto.DateOfBirth;
        
        var hasherResult = PasswordHasher.Hash(registroDto.Password);
        user.Password = hasherResult;

        user.UpdatedAt = TimeConstants.GetNow();

        var dbResult = await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveAsync();
        
        return dbResult.Id > 0;
    }
    
    public async Task<(bool Result, string Token)> LoginAsync(string mail, string password)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(x => x.Email == mail.ToLower());
        if(user is null) throw new NotFoundException("User not found");

        var hasherResult = PasswordHasher.Verify(password, user.Password);
        if (hasherResult == false) throw new CustomException(401, "Password has not been verified");

        string token = _tokenService.GenerateToken(user);

        return (Result: true, Token: token);
    }
}














    /*
     
     
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
    }*/