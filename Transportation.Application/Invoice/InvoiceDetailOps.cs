namespace Transportation.Application.Invoice;

public class InvoiceDetailOps
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
    public long InvoiceId { get; set; }
    public long ProductId { get; set; }
    public int Count { get; set; }
    public long UnitPrice { get; set; }
    public long Price { get; set; }
    public long Discount { get; set; }
    public long FinalPrice { get; set; }
    public string? Description { get; set; }
    public bool IsDeleted { get; set; }
}