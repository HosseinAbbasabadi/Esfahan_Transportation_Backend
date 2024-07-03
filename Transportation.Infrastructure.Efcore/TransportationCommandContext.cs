using Microsoft.EntityFrameworkCore;
using PhoenixFramework.Logging;
using Transport.Domain.VehicleTypeAgg;
using Transportation.Infrastructure.Efcore.Mapping;

namespace Transportation.Infrastructure.Efcore;

public class TransportationCommandContext : DbContext
{
    public DbSet<VehicleType> VehicleTypes { get; set; }
    public DbSet<OperationLog> OperationLogs { get; set; }
    
    public TransportationCommandContext(DbContextOptions<TransportationCommandContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(VehicleTypeMapping).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}