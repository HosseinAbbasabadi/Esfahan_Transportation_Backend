using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.UserAgg;

namespace UserManagement.Persistence.Mapping;

public class UserPasswordMapping : IEntityTypeConfiguration<UserPassword>
{
    public void Configure(EntityTypeBuilder<UserPassword> builder)
    {
        builder.ToTable("tbUserPasswords");
        builder.HasKey(x => x.Id);
    }
}