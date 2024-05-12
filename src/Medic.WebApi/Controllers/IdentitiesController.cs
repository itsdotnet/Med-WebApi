using Medic.Service.Exceptions;
using Medic.Service.Helpers;
using Medic.Service.Interfaces;
using Medic.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Medic.WebApi.Controllers;

public class IdentitiesController : BaseController
{
    private static IUserService _userService;
    private static IHttpContextAccessor Accessor;

    public IdentitiesController(IUserService userService, IHttpContextAccessor accessor)
    {
        _userService = userService;
        Accessor = accessor;
    }
    
    [HttpPost("identify")]
    public async Task<IActionResult> Identity()
    {
        if (Accessor.HttpContext is null)
            throw new CustomException(400, "User is not authorized");
        var userId = Accessor.HttpContext.User.FindFirst(u => u.Type == "Id").Value;
        var user = await _userService.GetByIdAsync(long.Parse(userId));
        return Ok(new Response()
        {
            StatusCode = 200,
            Message = "User role returned",
            Data = user.UserRole.ToString()
        });
    }
}