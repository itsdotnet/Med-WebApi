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

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> AuhenticateAsync(string email, string password)
    {
        if (Validator.IsValidEmail(email))
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.authService.LoginAsync(email, password)
            });

        return BadRequest(new Response
        {
            StatusCode = 400,
            Message = "Email is not valid"
        });  
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync(UserCreationDto dto)
    {
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.authService.RegisterAsync(dto)
        });
    }
}