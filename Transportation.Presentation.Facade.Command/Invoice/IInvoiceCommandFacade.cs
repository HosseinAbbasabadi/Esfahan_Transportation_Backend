using PhoenixFramework.Core;
using Transportation.Application.Invoice;

namespace Transportation.Presentation.Facade.Command.Invoice;

public interface IInvoiceCommandFacade : IFacadeService
{
    Task<Guid> Create(CreateInvoice command);
    Task Edit(EditInvoice command);
    Task Delete(Guid guid);
}