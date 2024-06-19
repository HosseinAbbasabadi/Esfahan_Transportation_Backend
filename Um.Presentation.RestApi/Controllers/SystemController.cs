using Microsoft.AspNetCore.Mvc;
using Um.Presentation.Facade.Contract.System;
using UserManagement.Query.Contracts.System;

namespace Um.Presentation.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SystemController : ControllerBase
{
    private readonly ISystemQueryFacade _systemQueryFacade;

    public SystemController(ISystemQueryFacade systemQueryFacade)
    {
        _systemQueryFacade = systemQueryFacade;
    }

    [HttpGet("GetForCombo")]
    public IActionResult GetForCombo([FromQuery] SystemSearchModel searchModel) =>
        new JsonResult(_systemQueryFacade.GetForCombo(searchModel));
}