using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.OrganizationChartAgg;

namespace UserManagement.Persistence.Mapping;

public class OrganizationChartMapping : IEntityTypeConfiguration<OrganizationChart>
{
    public void Configure(EntityTypeBuilder<OrganizationChart> builder)
    {
        builder.ToTable("tbOrganizationChart");
        builder.HasKey(x => x.Id);
    }
}