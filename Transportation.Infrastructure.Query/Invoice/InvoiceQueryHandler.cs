using Microsoft.EntityFrameworkCore;
using PhoenixFramework.Application;
using PhoenixFramework.Application.Query;
using Transportation.Application.Invoice;
using Transportation.Infrastructure.Efcore;

namespace Transportation.Infrastructure.Query.Invoice;

public class InvoiceQueryHandler :
    IQueryHandlerAsync<List<InvoiceViewModel>, InvoiceSearchModel>,
    IQueryHandlerAsync<EditInvoice, Guid>
{
    private readonly TransportationQueryContext _context;

    public InvoiceQueryHandler(TransportationQueryContext context)
    {
        _context = context;
    }

    public async Task<EditInvoice> Handle(Guid guid)
    {
        return await _context.Invoices
            .Select(x => new EditInvoice
            {
                Guid = x.Guid,
                Date = x.Date.ToFarsi(),
                No = x.No,
                CustomerId = x.CustomerId,
                Description = x.Description,
                Details = x.Details.Select(x => new InvoiceDetailOps
                {
                    Id = x.Id,
                    Guid = x.Guid,
                    ProductId = x.ProductId,
                    Count = x.Count,
                    UnitPrice = x.UnitPrice,
                    Price = x.Price,
                    Discount = x.Discount,
                    FinalPrice = x.FinalPrice,
                    Description = x.Description
                }).ToList()
            })
            .FirstAsync(x => x.Guid == guid);
    }

    public Task<List<InvoiceViewModel>> Handle(InvoiceSearchModel searchModel)
    {
        var query = _context.Invoices
            .Include(x => x.Details)
            .AsQueryable();

        if (searchModel.CustomerId != 0)
            query = query.Where(x => x.CustomerId == searchModel.CustomerId);

        if (!string.IsNullOrWhiteSpace(searchModel.FromDate))
            query = query.Where(x => x.Date.Date >= searchModel.FromDate.ToGeorgianDateTime().Date);

        if (!string.IsNullOrWhiteSpace(searchModel.ToDate))
            query = query.Where(x => x.Date.Date <= searchModel.ToDate.ToGeorgianDateTime().Date);

        if (searchModel.FromNo != 0)
            query = query.Where(x => x.No >= searchModel.FromNo);

        if (searchModel.ToNo != 0)
            query = query.Where(x => x.No <= searchModel.ToNo);

        return query.Select(x => new InvoiceViewModel
            {
                Guid = x.Guid,
                No = x.No,
                Date = x.Date.ToFarsi(),
                CustomerId = x.CustomerId,
                SumDiscount = x.SumDiscount,
                SumPrice = x.SumPrice,
                SumFinalPrice = x.SumFinalPrice,
                Description = x.Description
            }).AsNoTracking()
            .ToListAsync();
    }
}