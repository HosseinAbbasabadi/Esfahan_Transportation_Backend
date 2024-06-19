using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoenixFramework.Application.Query;
using UserManagement.Application.Contracts.Commands.User;
using UserManagement.Application.Contracts.Contracts;
using UserManagement.Application.Contracts.ViewModels;
using UserManagement.Query.Contracts.User;

namespace Um.Presentation.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IQueryBus _queryBus;
    private readonly IQueryBusAsync _queryBusAsync;
    private readonly IUserApplication _userApplication;

    public UserController(IUserApplication userApplication, IQueryBus queryBus, IQueryBusAsync queryBusAsync)
    {
        _userApplication = userApplication;
        _queryBus = queryBus;
        _queryBusAsync = queryBusAsync;
    }

    [HttpPost("Create")]
    public void Post([FromBody] CreateUser command)
    {
        _userApplication.Create(command);
    }

    [HttpPost("Deactivate/{guid:guid}")]
    public void Lock(Guid guid)
    {
        _userApplication.Lock(guid);
    }

    [HttpPost("Activate/{guid:guid}")]
    public void Unlock(Guid guid)
    {
        _userApplication.Unlock(guid);
    }

    [HttpGet("GetList")]
    public List<UserViewModel> Search()
    {
        var a = _userApplication.GetList();
        return a;
    }

    [HttpPost("Delete/{id:guid}")]
    public void Delete(Guid id)
    {
        _userApplication.Delete(id);
    }

    [HttpPost("OpenSession")]
    public void OpenSession([FromBody] OpenSession command)
    {
        command.ClientIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        _userApplication.OpenSession(command);
    }

    [HttpGet("GetBy/{guid:guid}")]
    public EditUser GetBy(Guid guid)
    {
        return _userApplication.GetBy(guid);
    }

    [HttpPost("Edit")]
    public void Put([FromBody] EditUser command)
    {
        _userApplication.Edit(command);
    }

    [HttpPost("ChangePassword")]
    public void ChangePassword(ChangePassword command)
    {
        _userApplication.ChangePassword(command);
    }

    [AllowAnonymous]
    [HttpPost("CloseSessions/{guid:guid}")]
    public void CloseSessions(Guid guid)
    {
        _userApplication.CloseSession(guid);
    }

    [HttpGet("GetForCombo")]
    public IActionResult GetForCombo()
    {
        return new JsonResult(_queryBus.Dispatch<List<UserComboModel>>());
    }

    [HttpGet("GetUserInformation")]
    public async Task<IActionResult> GetUserInformation()
    {
        return new JsonResult(await _queryBusAsync.Dispatch<UserInformationViewModel>());
    }

    [HttpGet("GetCurrentUserLastSessions")]
    public async Task<IActionResult> GetCurrentUserLastSessions()
    {
        return new JsonResult(await _queryBusAsync.Dispatch<List<UserSessionViewModel>>());
    }

    [HttpGet("HasActiveSession")]
    public async Task<IActionResult> HasActiveSession()
    {
        return new JsonResult(await _queryBusAsync.Dispatch<UserActiveSessionViewModel>());
    }

    [HttpPost("GetUserSessionsLog")]
    public async Task<IActionResult> GetUserSessionsLog([FromBody] UserSessionSearchModel searchModel)
    {
        return new JsonResult(
            await _queryBusAsync.Dispatch<List<UserSessionViewModel>, UserSessionSearchModel>(searchModel));
    }

    [HttpGet("GetForCombo/{guid:guid}")]
    public IActionResult GetForCombo(Guid guid)
    {
        return new JsonResult(_queryBus.Dispatch<List<UserComboModel>, Guid>(guid));
    }
}