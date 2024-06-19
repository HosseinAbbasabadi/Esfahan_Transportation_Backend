using Microsoft.AspNetCore.Mvc;
using UserManagement.Query.Contracts.Setting;
using Um.Presentation.Facade.Contract.Setting;
using UserManagement.Application.Contracts.Setting;

namespace Um.Presentation.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingController : ControllerBase
{
    private readonly ISettingQueryFacade _settingQueryFacade;
    private readonly ISettingCommandFacade _settingCommandFacade;

    public SettingController(ISettingQueryFacade settingQueryFacade, ISettingCommandFacade settingCommandFacade)
    {
        _settingQueryFacade = settingQueryFacade;
        _settingCommandFacade = settingCommandFacade;
    }

    [HttpPost("Edit")]
    public void Post([FromBody] UpdateSetting command) => _settingCommandFacade.UpdateSetting(command);

    [HttpGet("GetSettings")]
    public List<SettingViewModel> GetSettings() => _settingQueryFacade.GetSettings();
}