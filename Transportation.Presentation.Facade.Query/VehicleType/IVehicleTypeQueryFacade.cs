using PhoenixFramework.Core;
using PhoenixFramework.Identity;
using Transportation.Infrastructure.Query.VehicleType;

namespace Transportation.Presentation.Facade.Query.VehicleType;

public interface IVehicleTypeQueryFacade : IFacadeService
{
    // [HasPermission("VehicleType_List")]
    List<VehicleTypeViewModel> List(VehicleTypeSearchModel searchModel);

    [HasPermission("VehicleType_List")]
    List<VehicleTypeComboModel> Combo();

    [HasPermission("VehicleType_Edit")]
    VehicleTypeViewModel Detail(Guid guid);
}