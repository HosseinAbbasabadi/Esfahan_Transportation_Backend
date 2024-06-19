using PhoenixFramework.Application.Command;
using Um.Presentation.Facade.Contract.Setting;
using UserManagement.Application.Contracts.Setting;

namespace Um.Presentation.Facade.Command;

public class SettingCommandFacade : ISettingCommandFacade
{
    private readonly ICommandBus _commandBus;

    public SettingCommandFacade(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    public void UpdateSetting(UpdateSetting command) => _commandBus.Dispatch(command);
}