using PhoenixFramework.Core;
using PhoenixFramework.Identity;
using Transportation.Application.VehicleType;

namespace Transportation.Presentation.Facade.Command.VehicleType;

public interface IVehicleTypeCommandFacade : IFacadeService
{
    [HasPermission("VehicleType_Create")]
    Task<Guid> Create(CreateVehicleType command);

    [HasPermission("VehicleType_Edit")]
    Task Edit(EditVehicleType command);

    [HasPermission("VehicleType_Remove")]
    Task Remove(Guid guid);

    [HasPermission("VehicleType_Restore")]
    Task Restore(Guid guid);
}