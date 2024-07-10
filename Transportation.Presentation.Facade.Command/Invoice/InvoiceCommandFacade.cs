using PhoenixFramework.Application.Command;
using Transportation.Application.Invoice;

namespace Transportation.Presentation.Facade.Command.Invoice;

public class InvoiceCommandFacade : IInvoiceCommandFacade
{
    private readonly ICommandBusAsync _commandBusAsync;
    private readonly IResponsiveCommandBusAsync _responsiveCommandBusAsync;

    public InvoiceCommandFacade(ICommandBusAsync commandBusAsync, IResponsiveCommandBusAsync responsiveCommandBusAsync)
    {
        _commandBusAsync = commandBusAsync;
        _responsiveCommandBusAsync = responsiveCommandBusAsync;
    }

    public async Task<Guid> Create(CreateInvoice command) =>
        await _responsiveCommandBusAsync.Dispatch<CreateInvoice, Guid>(command);

    public async Task Edit(EditInvoice command) => await _commandBusAsync.Dispatch(command);

    public async Task Delete(Guid guid)
    {
        var command = new DeleteInvoice(guid);
        await _commandBusAsync.Dispatch(command);
    }
}