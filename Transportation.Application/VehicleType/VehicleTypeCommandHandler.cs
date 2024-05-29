using PhoenixFramework.Application.Command;
using PhoenixFramework.Identity;
using Transport.Domain.VehicleTypeAgg;

namespace Transportation.Application.VehicleType;

public class VehicleTypeCommandHandler :
    ICommandHandlerAsync<CreateVehicleType, Guid>,
    ICommandHandlerAsync<EditVehicleType>,
    ICommandHandlerAsync<RemoveVehicleType>,
    ICommandHandlerAsync<RestoreVehicleType>
{
    private readonly IClaimHelper _claimHelper;
    private readonly IVehicleTypeRepository _vehicleTypeRepository;

    public VehicleTypeCommandHandler(IVehicleTypeRepository vehicleTypeRepository, IClaimHelper claimHelper)
    {
        _vehicleTypeRepository = vehicleTypeRepository;
        _claimHelper = claimHelper;
    }

    public async Task<Guid> Handle(CreateVehicleType command)
    {
        var creator = _claimHelper.GetCurrentUserGuid();
        var vehicleType = new Transport.Domain.VehicleTypeAgg.VehicleType(creator, command.Title, command.Description);

        await _vehicleTypeRepository.CreateAsync(vehicleType);

        return vehicleType.Guid;
    }

    public async Task Handle(EditVehicleType command)
    {
        var editor = _claimHelper.GetCurrentUserGuid();
        var vehicleType = await _vehicleTypeRepository.LoadAsync(command.Guid);

        vehicleType.Edit(editor, command.Title, command.Description);
    }

    public async Task Handle(RemoveVehicleType command)
    {
        var editor = _claimHelper.GetCurrentUserGuid();
        var vehicleType = await _vehicleTypeRepository.LoadAsync(command.Guid);

        vehicleType.Remove(editor);
    }

    public async Task Handle(RestoreVehicleType command)
    {
        var editor = _claimHelper.GetCurrentUserGuid();
        var vehicleType = await _vehicleTypeRepository.LoadAsync(command.Guid);

        vehicleType.Restore(editor);
    }
}