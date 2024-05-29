using PhoenixFramework.EntityFramework;
using Transport.Domain.VehicleTypeAgg;

namespace Transportation.Infrastructure.Efcore.Repository;

public class VehicleTypeRepository : BaseRepository<long, VehicleType>, IVehicleTypeRepository
{
    public VehicleTypeRepository(TransportationCommandContext commandContext) : base(commandContext)
    {
    }
}