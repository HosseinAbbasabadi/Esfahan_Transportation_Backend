using PhoenixFramework.Application.Query;
using Transportation.Application.Invoice;
using Transportation.Infrastructure.Query.Invoice;

namespace Transportation.Presentation.Facade.Query.Invoice;

public class InvoiceQueryFacade : IInvoiceQueryFacade
{
    private readonly IQueryBusAsync _queryBus;

    public InvoiceQueryFacade(IQueryBusAsync queryBus)
    {
        _queryBus = queryBus;
    }

    public async Task<List<InvoiceViewModel>> List(InvoiceSearchModel searchModel) =>
        await _queryBus.Dispatch<List<InvoiceViewModel>, InvoiceSearchModel>(searchModel);

    public async Task<EditInvoice> Detail(Guid guid) => await _queryBus.Dispatch<EditInvoice, Guid>(guid);
}