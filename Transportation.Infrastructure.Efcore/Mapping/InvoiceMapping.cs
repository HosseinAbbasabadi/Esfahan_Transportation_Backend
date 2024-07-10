using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transport.Domain.InvoiceAgg;

namespace Transportation.Infrastructure.Efcore.Mapping;

public class InvoiceMapping : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("tbInvoice");
        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.IsActive);
        builder.Ignore(x => x.IsLocked);
        
        builder.HasMany<InvoiceDetail>()
            .WithOne(x => x.Invoice)
            .HasForeignKey(x => x.InvoiceId);
    }
}