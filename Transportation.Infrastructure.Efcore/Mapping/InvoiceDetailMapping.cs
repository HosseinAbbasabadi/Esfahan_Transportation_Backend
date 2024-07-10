using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transport.Domain.InvoiceAgg;

namespace Transportation.Infrastructure.Efcore.Mapping;

public class InvoiceDetailMapping : IEntityTypeConfiguration<InvoiceDetail>
{
    public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
    {
        builder.ToTable("tbInvoiceDetail");
        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.IsActive);

        builder.HasOne<Invoice>()
            .WithMany(x => x.Details)
            .HasForeignKey(x => x.InvoiceId);
    }
}