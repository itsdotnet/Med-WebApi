using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medic.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class BaseController : ControllerBase
{ }