namespace Transportation.Infrastructure.Query.Invoice;

public class InvoiceSearchModel
{
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }

    public long FromNo { get; set; }
    public long ToNo { get; set; }

    public long CustomerId { get; set; }
}