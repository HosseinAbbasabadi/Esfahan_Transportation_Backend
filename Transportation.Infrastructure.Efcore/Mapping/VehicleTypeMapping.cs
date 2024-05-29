using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transport.Domain.VehicleTypeAgg;

namespace Transportation.Infrastructure.Efcore.Mapping;

public class VehicleTypeMapping : IEntityTypeConfiguration<VehicleType>
{
    public void Configure(EntityTypeBuilder<VehicleType> builder)
    {
        builder.ToTable("tbVehicleType");
        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.IsLocked);
    }
}