using Medic.Service.Interfaces;

namespace Medic.WebApi.Controllers;

public class UsersController : BaseController
{
    private readonly IUserService userService;

    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }   
}
