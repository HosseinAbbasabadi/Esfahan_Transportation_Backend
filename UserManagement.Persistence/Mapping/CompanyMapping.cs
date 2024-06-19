using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.CompanyAgg;

namespace UserManagement.Persistence.Mapping;

public class CompanyMapping : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("tbCompanies");
        builder.HasKey(x => x.Id);
    }
}