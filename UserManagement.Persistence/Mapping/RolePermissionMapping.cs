using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.RoleAgg;

namespace UserManagement.Persistence.Mapping;

public class RolePermissionMapping : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("tbRolePermissions");
        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.Created);
        builder.Ignore(x => x.CreatedBy);
        builder.Ignore(x => x.IsActive);
        
        builder.HasOne(x => x.Role)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.RoleId);
    }
}