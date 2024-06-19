using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.ClassificationLevelAgg;
using UserManagement.Domain.CompanyAgg;
using UserManagement.Domain.Feature;
using UserManagement.Domain.OrganizationChartAgg;
using UserManagement.Domain.RoleAgg;
using UserManagement.Domain.UserAgg;
using UserManagement.Persistence.Mapping;

namespace UserManagement.Persistence;

public class UserManagementQueryContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolesPermissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserSystem> UserSystems { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<OrganizationChart> OrganizationCharts { get; set; }
    public DbSet<ClassificationLevel> ClassificationLevel { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<Domain.SystemAgg.System> Systems { get; set; }

    public UserManagementQueryContext(DbContextOptions<UserManagementQueryContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(UserMapping).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }
}