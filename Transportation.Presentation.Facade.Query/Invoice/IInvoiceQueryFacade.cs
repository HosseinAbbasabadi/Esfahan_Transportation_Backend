using PhoenixFramework.Core;
using Transportation.Application.Invoice;
using Transportation.Infrastructure.Query.Invoice;

namespace Transportation.Presentation.Facade.Query.Invoice;

public interface IInvoiceQueryFacade : IFacadeService
{
    Task<List<InvoiceViewModel>> List(InvoiceSearchModel searchModel);
    Task<EditInvoice> Detail(Guid guid);
}