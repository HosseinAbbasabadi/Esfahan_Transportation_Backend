using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.UserAgg;

namespace UserManagement.Persistence.Mapping;

public class UserSystemMapping : IEntityTypeConfiguration<UserSystem>
{
    public void Configure(EntityTypeBuilder<UserSystem> builder)
    {
        builder.ToTable("tbUserSystems");
        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.IsActive);
        builder.Ignore(x => x.Created);
        builder.Ignore(x => x.CreatedBy);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Systems)
            .HasForeignKey(x => x.UserId);
    }
}