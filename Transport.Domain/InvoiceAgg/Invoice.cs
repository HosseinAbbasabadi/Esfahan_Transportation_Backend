using PhoenixFramework.Domain;

namespace Transport.Domain.InvoiceAgg;

public class Invoice : AuditableAggregateRootBase<long>
{
    public long No { get; private set; }
    public DateTime Date { get; private set; }
    public long CustomerId { get; private set; }
    public string? Description { get; private set; }
    public long SumPrice { get; private set; }
    public long SumDiscount { get; private set; }
    public long SumFinalPrice { get; private set; }
    public List<InvoiceDetail> Details { get; private set; }

    protected Invoice()
    {
    }

    public Invoice(Guid createdBy, long no, DateTime date, long customerId, string? description) : base(createdBy)
    {
        No = no;
        Date = date;
        CustomerId = customerId;
        Description = description;
    }

    public void SetDetails(List<InvoiceDetail> details)
    {
        Details = details;

        CalculateSummary();
    }

    private void CalculateSummary()
    {
        SumPrice = Details.Sum(x => x.Price);
        SumDiscount = Details.Sum(x => x.Discount);
        SumFinalPrice = Details.Sum(x => x.FinalPrice);
    }
}