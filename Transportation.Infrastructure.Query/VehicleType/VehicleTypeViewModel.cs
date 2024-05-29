using PhoenixFramework.Application.Query;
using PhoenixFramework.Company.Query;

namespace Transportation.Infrastructure.Query.VehicleType;

public class VehicleTypeViewModel : ViewModelAbilities, IQuery
{
    public string Title { get; set; }
    public string? Description { get; set; }
}