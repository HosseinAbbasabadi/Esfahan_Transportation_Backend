using PhoenixFramework.Core;
using UserManagement.Query.Contracts.Setting;

namespace Um.Presentation.Facade.Contract.Setting;

public interface ISettingQueryFacade : IFacadeService
{
    List<SettingViewModel> GetSettings();
}