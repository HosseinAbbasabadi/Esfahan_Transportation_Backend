using Microsoft.AspNetCore.Mvc;
using PhoenixFramework.Application.Query;
using UserManagement.Application.Contracts.Commands;
using UserManagement.Application.Contracts.Contracts;
using UserManagement.Application.Contracts.SearchModels;
using UserManagement.Application.Contracts.ViewModels;

namespace Um.Presentation.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeatureController : ControllerBase
{
    private readonly IQueryBus _queryBus;
    private readonly IQueryBusAsync _queryBusAsync;
    private readonly IFeatureApplication _featureApplication;

    public FeatureController(IFeatureApplication featureApplication, IQueryBus queryBus, IQueryBusAsync queryBusAsync)
    {
        _featureApplication = featureApplication;
        _queryBus = queryBus;
        _queryBusAsync = queryBusAsync;
    }

    [HttpPost("UpdateClassificationLevel")]
    public void UpdateClassificationLevel([FromBody] UpdateClassificationLevel command)
    {
        _featureApplication.UpdateClassificationLevel(command);
    }

    [HttpGet("GetList/{guid?}")]
    public IActionResult GetList(Guid? guid)
    {
        return new JsonResult(_featureApplication.GetAll(guid));
    }

    [HttpGet("GetUserPermissions")]
    public IActionResult GetUserPermissions()
    {
        return new JsonResult(_queryBus.Dispatch<string>());
    }

    [HttpGet("GetForClassificationLevel")]
    public IActionResult GetForClassificationLevel()
    {
        return new JsonResult(_queryBus.Dispatch<List<FeatureViewModel>, FeatureSearchModel>(new FeatureSearchModel()));
    }

    [HttpGet("GetClassificationLevelByTitle")]
    public async Task<IActionResult> GetClassificationLevelByTitle(
        [FromQuery] FeatureClassificationLevelSearchModel searchModel)
    {
        return new JsonResult(await _queryBusAsync
            .Dispatch<FeatureClassificationLevelViewModel, FeatureClassificationLevelSearchModel>(searchModel));
    }
}