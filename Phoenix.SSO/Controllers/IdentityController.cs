using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PhoenixFramework.Identity;

namespace Phoenix.SSO.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = Roles.Everybody)]
public class IdentityController : ControllerBase
{
    private readonly IClaimHelper _claimHelper;

    public IdentityController(IClaimHelper claimHelper)
    {
        _claimHelper = claimHelper;
    }

    [HttpGet("GetFullname")]
    public string GetFullname()
    {
        // return _userApplication.GetFullname();
        return "";
    }

    [HttpGet("GetUserIdAndRole")]
    public IActionResult GetUserIdAndRole()
    {
        var role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        var id = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        return new JsonResult(new { id, role });
    }
}