using System.Text.Json.Serialization;
using Medic.Service.DTOs.Users;
using Medic.WebApi.Models;
using Medic.Service.Helpers;
using Microsoft.AspNetCore.Mvc;
using Medic.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
namespace Medic.WebApi.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(LoginModel model)
    {

        if (Validator.IsValidEmail(model.Email))    
        {
            var serviceResult = await authService.LoginAsync(model.Email, model.Password);
            if(serviceResult is not null)
                return Ok(new Response()
                {
                    StatusCode = 200,
                    Message = "Login successful",
                    Data = serviceResult
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
                Message = "User registered successfully",
                Data = serviceResult
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

    [HttpPost("login-google")]
    [AllowAnonymous]
    public async Task LoginGoogle()
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
            new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
    }
    
    [HttpPost("google-response")]
    [AllowAnonymous]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(c => new
        {
            c.Issuer,
            c.OriginalIssuer,
            c.Type,
            c.Value
        });
        JsonResult s = new JsonResult(claims);
        return Ok(claims);
    }

    //
    // [HttpPost("register/send-code")]
    // [AllowAnonymous]
    // public async Task<IActionResult> SendCodeRegisterAsync(string mail)
    // {
    //     var serviceResult = await authService.SendCodeForRegisterAsync(mail);
    //     return Ok(new Response()
    //     {
    //         StatusCode = 200,
    //         Message = "Code sent successfully",
    //         Data = serviceResult
    //     });
    // }
    //
    //
    // [HttpPost("register/verify")]
    // [AllowAnonymous]
    // public async Task<IActionResult> VerifyRegisterAsync(VerifyModel model)
    // {
    //     var serviceResult = await authService.VerifyRegisterAsync(model.Email, int.Parse(model.Code));
    //     if (serviceResult.Result == false)
    //     {
    //         return BadRequest(new Response()
    //         {
    //             StatusCode = 401,
    //             Message = "Invalid code"
    //         });
    //     }
    //     return Ok(new Response()
    //     {
    //         StatusCode = 200,
    //         Message = "User verfied successfully",
    //         Data = serviceResult.Token
    //     });
    // }
    //

}