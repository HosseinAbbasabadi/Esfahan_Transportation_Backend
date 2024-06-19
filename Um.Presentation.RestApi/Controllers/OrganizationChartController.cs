using Microsoft.AspNetCore.Mvc;
using Um.Presentation.Facade.Contract.OrganizationChart;
using UserManagement.Application.Contracts.OrganizationChart;
using UserManagement.Query.Contracts.OrganizationChart;

namespace Um.Presentation.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationChartController : ControllerBase
{
    private readonly IOrganizationChartCommandFacade _organizationChartCommandFacade;
    private readonly IOrganizationChartQueryFacade _organizationChartQueryFacade;

    public OrganizationChartController(IOrganizationChartCommandFacade organizationChartCommandFacade,
        IOrganizationChartQueryFacade organizationChartQueryFacade)
    {
        _organizationChartCommandFacade = organizationChartCommandFacade;
        _organizationChartQueryFacade = organizationChartQueryFacade;
    }

    [HttpPost("ChangeParent")]
    public void ChangeParent([FromBody] ChangeOrganizationChartParent command)
    {
        _organizationChartCommandFacade.Create(command);
    }

    [HttpPost("Edit")]
    public IActionResult Edit([FromBody] EditOrganizationChart command)
    {
        return new JsonResult(_organizationChartCommandFacade.Edit(command));
    }

    [HttpPost("Delete/{guid:guid}")]
    public void Delete(Guid guid)
    {
        _organizationChartCommandFacade.Delete(guid);
    }

    [HttpGet("GetForTree")]
    public IActionResult GetForTree([FromQuery] OrganizationChartSearchModel searchModel)
    {
        return new JsonResult(_organizationChartQueryFacade.GetForTree(searchModel));
    }

    [HttpGet("GetForChart")]
    public IActionResult GetForChart([FromQuery] OrganizationChartSearchModel searchModel)
    {
        return new JsonResult(_organizationChartQueryFacade.GetForChart(searchModel));
    }
}