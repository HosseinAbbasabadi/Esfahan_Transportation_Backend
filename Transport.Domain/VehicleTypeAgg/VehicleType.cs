using PhoenixFramework.Domain;
using Transport.Domain.VehicleTypeAgg.Service;

namespace Transport.Domain.VehicleTypeAgg;

public class VehicleType : AuditableAggregateRootBase<long>
{
    public string Title { get; private set; }
    public string? Description { get; private set; }

    public VehicleType(Guid creator, string title, string? description, IVehicleTypeService vehicleTypeService) :
        base(creator)
    {
        vehicleTypeService.ThrowWhenTitleIsDuplicated(title);

        Title = title;
        Description = description;
    }

    public void Edit(Guid editor, string title, string? description, IVehicleTypeService vehicleTypeService)
    {
        vehicleTypeService.ThrowWhenTitleIsDuplicated(title, Guid);

        Title = title;
        Description = description;

        Modified(editor);

        // Remove(editor);
        // Restore(editor);
    }
}