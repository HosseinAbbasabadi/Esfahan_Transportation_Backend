using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.UserAgg;

namespace UserManagement.Persistence.Mapping;

public class UserRoleMapping : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("tbUserRoles");
        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.IsActive);
        builder.Ignore(x => x.Created);
        builder.Ignore(x => x.CreatedBy);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Roles)
            .HasForeignKey(x => x.UserId);
    }
}