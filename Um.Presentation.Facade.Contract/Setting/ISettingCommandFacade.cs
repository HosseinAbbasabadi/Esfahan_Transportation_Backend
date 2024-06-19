using PhoenixFramework.Core;
using UserManagement.Application.Contracts.Setting;

namespace Um.Presentation.Facade.Contract.Setting;

public interface ISettingCommandFacade : IFacadeService
{
    void UpdateSetting(UpdateSetting command);
}