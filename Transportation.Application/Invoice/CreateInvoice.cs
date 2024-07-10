using PhoenixFramework.Application.Command;

namespace Transportation.Application.Invoice;

public class CreateInvoice : ICommand
{
    public long No { get; set; }
    public string Date { get; set; }

    public long CustomerId { get; set; }

    // public long SumPrice { get;  set; }
    // public long SumDiscount { get;  set; }
    // public long SumFinalPrice { get;  set; }
    public string? Description { get; set; }
    public List<InvoiceDetailOps> Details { get; set; }
}