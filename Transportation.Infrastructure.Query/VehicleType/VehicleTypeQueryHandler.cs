using PhoenixFramework.Application.Query;
using PhoenixFramework.Dapper;

namespace Transportation.Infrastructure.Query.VehicleType;

public class VehicleTypeQueryHandler :
    IQueryHandler<List<VehicleTypeViewModel>, VehicleTypeSearchModel>,
    IQueryHandler<List<VehicleTypeComboModel>>,
    IQueryHandler<VehicleTypeViewModel, Guid>
{
    private const string SpName = "spGetVehicleTypeFor";
    private readonly BaseDapperRepository _dapper;

    public VehicleTypeQueryHandler(BaseDapperRepository dapper)
    {
        _dapper = dapper;
    }

    public List<VehicleTypeViewModel> Handle(VehicleTypeSearchModel searchModel)
    {
        return _dapper.SelectFromSp<VehicleTypeViewModel>(SpName, new
        {
            Type = QueryOutputs.List
        });

        return _dapper.Select<VehicleTypeViewModel>(
            $"SELECT Guid, Title, Description FROM tbVehicleType WHERE IsRemoved = {searchModel.IsRemoved}");

        // return _context.VehicleTypes
        //     .Where(x => x.IsRemoved == searchModel.IsRemoved)
        //     .Select(x => new VehicleTypeViewModel
        //     {
        //         Guid = x.Guid,
        //         Title = x.Title,
        //         Description = x.Description
        //     })
        //     .AsNoTracking()
        //     .ToList();
    }

    public List<VehicleTypeComboModel> Handle()
    {
        return _dapper.SelectFromSp<VehicleTypeComboModel>(SpName, new
        {
            Type = QueryOutputs.Combo
        });
    }

    public VehicleTypeViewModel Handle(Guid guid)
    {
        return _dapper.SelectFromSpFirstOrDefault<VehicleTypeViewModel>(SpName, new
        {
            Type = QueryOutputs.Edit,
            Guid = guid
        });
    }
}