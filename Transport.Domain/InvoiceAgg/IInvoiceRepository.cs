using PhoenixFramework.Domain;

namespace Transport.Domain.InvoiceAgg;

public interface IInvoiceRepository : IRepository<long, Invoice>
{
    
}