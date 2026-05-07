using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace CareerHub.API.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected Guid UserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
