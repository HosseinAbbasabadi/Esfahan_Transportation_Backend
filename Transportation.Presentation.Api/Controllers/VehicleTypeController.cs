using Microsoft.AspNetCore.Mvc;
using Transportation.Application.VehicleType;
using Transportation.Infrastructure.Query.VehicleType;
using Transportation.Presentation.Facade.Command.VehicleType;
using Transportation.Presentation.Facade.Query.VehicleType;

namespace Transportation.Presentation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleTypeController : ControllerBase
{
    private readonly IVehicleTypeCommandFacade _commandFacade;
    private readonly IVehicleTypeQueryFacade _queryFacade;
    // private readonly IResponsiveCommandBusAsync _responsiveCommandBusAsync;

    public VehicleTypeController(IVehicleTypeCommandFacade commandFacade, IVehicleTypeQueryFacade queryFacade)
    {
        _commandFacade = commandFacade;
        _queryFacade = queryFacade;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] CreateVehicleType command)
    {
        return new JsonResult(await _commandFacade.Create(command));
        // return new JsonResult(await _responsiveCommandBusAsync.Dispatch<CreateVehicleType, Guid>(command));
    }

    [HttpPost("Edit")]
    public void Edit([FromBody] EditVehicleType command) =>
        _commandFacade.Edit(command);

    [HttpPost("Remove/{guid:guid}")]
    public void Activate(Guid guid) =>
        _commandFacade.Remove(guid);

    [HttpPost("Restore/{guid:guid}")]
    public void Deactivate(Guid guid) =>
        _commandFacade.Restore(guid);

    [HttpGet("GetList")]
    public IActionResult List([FromQuery] VehicleTypeSearchModel searchModel)
        => new JsonResult(_queryFacade.List(searchModel));

    [HttpGet("GetForEdit/{guid:guid}")]
    public IActionResult GetDetails(Guid guid)
        => new JsonResult(_queryFacade.Detail(guid));
}