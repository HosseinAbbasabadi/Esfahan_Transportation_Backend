using PhoenixFramework.Application.Query;
using Transportation.Infrastructure.Query.VehicleType;

namespace Transportation.Presentation.Facade.Query.VehicleType;

public class VehicleTypeQueryFacade : IVehicleTypeQueryFacade
{
    private readonly IQueryBus _queryBus;
    private readonly IQueryBusAsync _queryBusAsync;

    public VehicleTypeQueryFacade(IQueryBus queryBus, IQueryBusAsync queryBusAsync)
    {
        _queryBus = queryBus;
        _queryBusAsync = queryBusAsync;
    }

    public List<VehicleTypeViewModel> List(VehicleTypeSearchModel searchModel)
    {
        return _queryBus.Dispatch<List<VehicleTypeViewModel>, VehicleTypeSearchModel>(searchModel);
    }

    public List<VehicleTypeComboModel> Combo()
    {
        return _queryBus.Dispatch<List<VehicleTypeComboModel>>();
    }

    public VehicleTypeViewModel Detail(Guid guid)
    {
        return _queryBus.Dispatch<VehicleTypeViewModel, Guid>(guid);
    }
}