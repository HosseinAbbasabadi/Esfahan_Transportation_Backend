using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.UserAgg;

namespace UserManagement.Persistence.Mapping;

public class UserSessionMapping : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("tbUserSessions");
        builder.HasKey(x => x.Id);
    }
}