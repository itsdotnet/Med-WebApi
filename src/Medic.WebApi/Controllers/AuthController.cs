using Medic.Service.DTOs.Users;
using Medic.WebApi.Models;
using Medic.Service.Helpers;
using Microsoft.AspNetCore.Mvc;
using Medic.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
namespace Medic.WebApi.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync(UserCreationDto registerDto)
    {
        var serviceResult = await authService.RegisterAsync(registerDto);
        if (Validator.IsValidEmail(registerDto.Email))
        {        
            return Ok(new Response()
            {
                StatusCode = 200,
                Message = "User cached successfully",
                Data = $"Cached for {serviceResult} minutes"
            });
        }
        else
        {
            return BadRequest(new Response()
            {
                StatusCode = 400,
                Message = "Invalid email"
            });
        }
    }


    [HttpPost("register/send-code")]
    [AllowAnonymous]
    public async Task<IActionResult> SendCodeRegisterAsync(string mail)
    {
        var serviceResult = await authService.SendCodeForRegisterAsync(mail);
        return Ok(new Response()
        {
            StatusCode = 200,
            Message = "Code sent successfully",
            Data = serviceResult
        });
    }
    

    [HttpPost("register/verify")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyRegisterAsync(VerifyModel model)
    {
        var serviceResult = await authService.VerifyRegisterAsync(model.Email, model.Code);
        if (serviceResult.Result == false)
        {
            return BadRequest(new Response()
            {
                StatusCode = 401,
                Message = "Invalid code"
            });
        }
        return Ok(new Response()
        {
            StatusCode = 200,
            Message = "User verfied successfully",
            Data = serviceResult.Token
        });
    }


    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(LoginModel model)
    {

        if (Validator.IsValidEmail(model.Email))    
        {
            var serviceResult = await authService.LoginAsync(model.Email, model.Password);
            if(serviceResult.Result)
                return Ok(new Response()
                {
                    StatusCode = 200,
                    Message = "Login successful",
                    Data = serviceResult.Token
                });
            else
                return BadRequest(new Response()
                {
                    StatusCode = 401,
                    Message = "Invalid password"
                });
        }
        else
        {
            return BadRequest(new Response()
            {
                StatusCode = 400,
                Message = "Invalid email"
            });
        }
    }
}