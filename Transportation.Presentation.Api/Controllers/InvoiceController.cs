using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transportation.Application.Invoice;
using Transportation.Infrastructure.Query.Invoice;
using Transportation.Presentation.Facade.Command.Invoice;
using Transportation.Presentation.Facade.Query.Invoice;

namespace Transportation.Presentation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceCommandFacade _commandFacade;
    private readonly IInvoiceQueryFacade _queryFacade;
    // private readonly IResponsiveCommandBusAsync _responsiveCommandBusAsync;

    public InvoiceController(IInvoiceCommandFacade commandFacade, IInvoiceQueryFacade queryFacade)
    {
        _commandFacade = commandFacade;
        _queryFacade = queryFacade;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateInvoice command) =>
        new JsonResult(await _commandFacade.Create(command));

    [HttpPost("Edit")]
    public async Task Edit([FromBody] EditInvoice command) => await _commandFacade.Edit(command);

    [HttpPost("Delete/{guid:guid}")]
    public async Task Activate(Guid guid) =>
        await _commandFacade.Delete(guid);

    [HttpGet("GetList")]
    public async Task<IActionResult> List([FromQuery] InvoiceSearchModel searchModel)
        => new JsonResult(await _queryFacade.List(searchModel));

    [HttpGet("GetForEdit/{guid:guid}")]
    public async Task<IActionResult> GetDetails(Guid guid)
        => new JsonResult(await _queryFacade.Detail(guid));
}