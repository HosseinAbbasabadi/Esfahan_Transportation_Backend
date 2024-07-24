using PhoenixFramework.Domain;

namespace Transport.Domain.InvoiceAgg;

public class InvoiceDetail : EntityBase<long>
{
    public Guid Guid { get; private set; }
    public long InvoiceId { get; private set; }
    public long ProductId { get; private set; }
    public int Count { get; private set; }
    public long UnitPrice { get; private set; }
    public long Price { get; private set; }
    public long Discount { get; private set; }
    public long FinalPrice { get; private set; }
    public string? Description { get; private set; }
    public Invoice? Invoice { get; private set; }

    protected InvoiceDetail()
    {
    }

    public InvoiceDetail(Guid creator, long productId, int count, long unitPrice, long price,
        long discount, long finalPrice, string? description) : base(creator)
    {
        Guid = Guid.NewGuid();
        ProductId = productId;
        Count = count;
        UnitPrice = unitPrice;
        Price = price;
        Discount = discount;
        FinalPrice = finalPrice;
        Description = description;
    }
    
    public void Edit(long productId, int count, long unitPrice, long price,
        long discount, long finalPrice, string? description)
    {
        ProductId = productId;
        Count = count;
        UnitPrice = unitPrice;
        Price = price;
        Discount = discount;
        FinalPrice = finalPrice;
        Description = description;
    }
}