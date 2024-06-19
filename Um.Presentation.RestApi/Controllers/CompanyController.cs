using Microsoft.AspNetCore.Mvc;
using Um.Presentation.Facade.Contract.Company;
using UserManagement.Application.Contracts.Company;
using UserManagement.Query.Contracts.Company;

namespace Um.Presentation.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyCommandFacade _companyCommandFacade;
    private readonly ICompanyQueryFacade _companyQueryFacade;

    public CompanyController(ICompanyCommandFacade companyCommandFacade, ICompanyQueryFacade companyQueryFacade)
    {
        _companyCommandFacade = companyCommandFacade;
        _companyQueryFacade = companyQueryFacade;
    }

    [HttpPost("Create")]
    public IActionResult Create([FromForm] CreateCompany command)
    {
        return new JsonResult(_companyCommandFacade.Create(command));
    }

    [HttpPost("Edit")]
    public void Edit([FromForm] EditCompany command)
    {
        _companyCommandFacade.Edit(command);
    }

    [HttpPost("Delete/{guid:guid}")]
    public void Delete(Guid guid)
    {
        _companyCommandFacade.Delete(guid);
    }

    [HttpPost("Deactivate/{guid:guid}")]
    public void Remove(Guid guid)
    {
        _companyCommandFacade.Deactivate(guid);
    }

    [HttpPost("Activate/{guid:guid}")]
    public void Restore(Guid guid)
    {
        _companyCommandFacade.Activate(guid);
    }
    
    [HttpGet("GetList")]
    public IActionResult GetList()
    {
        return new JsonResult(_companyQueryFacade.GetList());
    }

    [HttpGet("GetForEdit/{guid:guid}")]
    public IActionResult GetForEdit(Guid guid)
    {
        return new JsonResult(_companyQueryFacade.GetForEdit(guid));
    }

    [HttpGet("GetForCombo")]
    public IActionResult GetForCombo([FromQuery] CompanySearchModel searchModel)
    {
        return new JsonResult(_companyQueryFacade.GetForCombo(searchModel));
    }

    [HttpGet("GetForAdminCombo")]
    public IActionResult GetForAdminCombo()
    {
        return new JsonResult(_companyQueryFacade.GetForAdminCombo());
    }

    [HttpGet("GetForChart")]
    public IActionResult GetForChart([FromQuery] CompanySearchModel searchModel)
    {
        return new JsonResult(_companyQueryFacade.GetForChart(searchModel));
    }
}