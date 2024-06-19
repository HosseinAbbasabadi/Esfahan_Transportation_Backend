using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.ClassificationLevelAgg;

namespace UserManagement.Persistence.Mapping;

public class ClassificationLevelMapping : IEntityTypeConfiguration<ClassificationLevel>
{
    public void Configure(EntityTypeBuilder<ClassificationLevel> builder)
    {
        builder.ToTable("tbClassificationLevel");
        builder.HasKey(x => x.Id);
        
        builder.Ignore(x => x.IsActive);
        builder.Ignore(x => x.Created);
        builder.Ignore(x => x.CreatedBy);
    }
}