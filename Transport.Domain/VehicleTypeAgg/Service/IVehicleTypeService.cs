using PhoenixFramework.Core;

namespace Transport.Domain.VehicleTypeAgg.Service;

public interface IVehicleTypeService : IDomainService
{
    Task ThrowWhenTitleIsDuplicated(string title, Guid? guid = null);
}