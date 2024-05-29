using PhoenixFramework.Application.Command;
using Transportation.Application.VehicleType;

namespace Transportation.Presentation.Facade.Command.VehicleType;

public class VehicleTypeCommandFacade : IVehicleTypeCommandFacade
{
    private readonly ICommandBus _commandBus;
    private readonly ICommandBusAsync _commandBusAsync;
    private readonly IResponsiveCommandBus _responsiveCommandBus;
    private readonly IResponsiveCommandBusAsync _responsiveCommandBusAsync;

    public VehicleTypeCommandFacade(ICommandBus commandBus, ICommandBusAsync commandBusAsync,
        IResponsiveCommandBus responsiveCommandBus, IResponsiveCommandBusAsync responsiveCommandBusAsync)
    {
        _commandBus = commandBus;
        _commandBusAsync = commandBusAsync;
        _responsiveCommandBus = responsiveCommandBus;
        _responsiveCommandBusAsync = responsiveCommandBusAsync;
    }

    public async Task<Guid> Create(CreateVehicleType command)
    {
        return await _responsiveCommandBusAsync.Dispatch<CreateVehicleType, Guid>(command);
    }

    public async Task Edit(EditVehicleType command)
    {
        await _commandBusAsync.Dispatch(command);
        // await _commandBusAsync.Dispatch(command);
    }

    public async Task Remove(Guid guid)
    {
        var command = new RemoveVehicleType
        {
            Guid = guid
        };

        await _commandBusAsync.Dispatch(command);
    }

    public async Task Restore(Guid guid)
    {
        var command = new RestoreVehicleType()
        {
            Guid = guid
        };

        await _commandBusAsync.Dispatch(command);
    }
}