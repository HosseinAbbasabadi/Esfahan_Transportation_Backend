using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.Feature;

namespace UserManagement.Persistence.Mapping;

public class FeatureMapping : IEntityTypeConfiguration<Feature>
{
    public void Configure(EntityTypeBuilder<Feature> builder)
    {
        builder.ToTable("tbFeatures");
        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.Created);
        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.IsActive);
        
        builder.HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId);
    }
}