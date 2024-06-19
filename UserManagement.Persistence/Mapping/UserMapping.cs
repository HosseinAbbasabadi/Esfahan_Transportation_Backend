using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.UserAgg;

namespace UserManagement.Persistence.Mapping;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("tbUsers");

        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Roles)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.Sessions)
            .WithOne(x => x.User)
            .HasPrincipalKey(x => x.Guid);

        builder.HasMany(x => x.Systems)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        builder.Ignore(x => x.Claims);
    }
}