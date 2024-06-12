using System.Linq.Expressions;
using PhoenixFramework.Core.Exceptions;
using PhoenixFramework.Domain.Specification;

namespace Transport.Domain.VehicleTypeAgg.Service;

public class VehicleTypeService : IVehicleTypeService
{
    private Expression<Func<VehicleType, bool>> Predicate;
    private readonly IVehicleTypeRepository _vehicleTypeRepository;

    public VehicleTypeService(IVehicleTypeRepository vehicleTypeRepository)
    {
        _vehicleTypeRepository = vehicleTypeRepository;
    }

    public async Task ThrowWhenTitleIsDuplicated(string title, Guid? guid = null)
    {
        Predicate = x => x.Title == title;
        if (guid is not null)
            Predicate = Predicate.And(x => x.Guid != guid);

        if (await _vehicleTypeRepository.ExistsAsync(Predicate))
            throw new BusinessException("01", "title is duplicated");
    }
}