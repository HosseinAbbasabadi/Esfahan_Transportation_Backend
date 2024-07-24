namespace Transportation.Infrastructure.Query.Invoice;

public class InvoiceViewModel
{
    public Guid Guid { get; set; }
    public long No { get; set; }
    public string Date { get; set; }
    public long CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string? Description { get; set; }
    public long SumPrice { get; set; }
    public long SumDiscount { get; set; }
    public long SumFinalPrice { get; set; }
}