using PhoenixFramework.EntityFramework;
using Transport.Domain.InvoiceAgg;

namespace Transportation.Infrastructure.Efcore.Repository;

public class InvoiceRepository : BaseRepository<long, Invoice>, IInvoiceRepository
{
    private readonly TransportationCommandContext _context;

    public InvoiceRepository(TransportationCommandContext context) : base(context)
    {
        _context = context;
    }
}