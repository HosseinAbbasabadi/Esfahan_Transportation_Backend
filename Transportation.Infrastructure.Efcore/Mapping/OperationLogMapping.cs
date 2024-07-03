using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoenixFramework.Logging;

namespace Transportation.Infrastructure.Efcore.Mapping;

public class OperationLogMapping : IEntityTypeConfiguration<OperationLog>
{
    public void Configure(EntityTypeBuilder<OperationLog> builder)
    {
        builder.ToTable("tbOperationLog");
        builder.HasKey(x => x.Guid);
    }
}