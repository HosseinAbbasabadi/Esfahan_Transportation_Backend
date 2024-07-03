using Microsoft.EntityFrameworkCore;
using PhoenixFramework.Logging;
using Transport.Domain.VehicleTypeAgg;
using Transportation.Infrastructure.Efcore.Mapping;

namespace Transportation.Infrastructure.Efcore;

public class TransportationQueryContext : DbContext
{
    public DbSet<VehicleType> VehicleTypes { get; set; }
    public DbSet<OperationLog> OperationLogs { get; set; }

    public TransportationQueryContext(DbContextOptions<TransportationQueryContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(VehicleTypeMapping).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}