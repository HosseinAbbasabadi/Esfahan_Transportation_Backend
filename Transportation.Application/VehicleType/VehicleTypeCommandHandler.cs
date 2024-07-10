using PhoenixFramework.Application.Command;
using PhoenixFramework.Core.Exceptions;
using PhoenixFramework.Identity;
using Transport.Domain.VehicleTypeAgg;
using Transport.Domain.VehicleTypeAgg.Service;

namespace Transportation.Application.VehicleType;

public class VehicleTypeCommandHandler :
    ICommandHandlerAsync<CreateVehicleType, Guid>,
    ICommandHandlerAsync<EditVehicleType>,
    ICommandHandlerAsync<RemoveVehicleType>,
    ICommandHandlerAsync<RestoreVehicleType>
{
    private readonly IClaimHelper _claimHelper;
    private readonly IVehicleTypeRepository _vehicleTypeRepository;
    private readonly IVehicleTypeService _vehicleTypeService;

    public VehicleTypeCommandHandler(IVehicleTypeRepository vehicleTypeRepository, IClaimHelper claimHelper,
        IVehicleTypeService vehicleTypeService)
    {
        _vehicleTypeRepository = vehicleTypeRepository;
        _claimHelper = claimHelper;
        _vehicleTypeService = vehicleTypeService;
    }

    public async Task<Guid> Handle(CreateVehicleType command)
    {
        //_vehicleTypeService.ThrowWhenTitleIsDuplicated(command.Title);

        var creator = _claimHelper.GetCurrentUserGuid();
        
        var vehicleType =
            new Transport.Domain.VehicleTypeAgg.VehicleType(creator, command.Title, command.Description,
                _vehicleTypeService);

        await _vehicleTypeRepository.CreateAsync(vehicleType);

        return vehicleType.Guid;
    }

    public async Task Handle(EditVehicleType command)
    {
        //_vehicleTypeService.ThrowWhenTitleIsDuplicated(command.Title, command.Guid);

        var editor = _claimHelper.GetCurrentUserGuid();
        var vehicleType = await _vehicleTypeRepository.LoadAsync(command.Guid);

        vehicleType.Edit(editor, command.Title, command.Description, _vehicleTypeService);
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