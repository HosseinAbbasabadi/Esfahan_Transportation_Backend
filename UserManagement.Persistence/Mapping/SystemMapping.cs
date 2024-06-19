using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserManagement.Persistence.Mapping;

public class SystemMapping : IEntityTypeConfiguration<Domain.SystemAgg.System>
{
    public void Configure(EntityTypeBuilder<Domain.SystemAgg.System> builder)
    {
        builder.ToTable("tbSystems");
        builder.HasKey(x => x.Id);
    }
}