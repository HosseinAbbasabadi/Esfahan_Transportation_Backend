using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.RoleAgg;

namespace UserManagement.Persistence.Mapping;

public class RoleMapping : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("tbRoles");
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Permissions)
            .WithOne(x => x.Role)
            .HasForeignKey(x => x.RoleId);
    }
}