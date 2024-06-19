using PhoenixFramework.Application.Query;
using Um.Presentation.Facade.Contract.Setting;
using UserManagement.Query.Contracts.Setting;

namespace Um.Presentation.Facade.Query;

public class SettingQueryFacade : ISettingQueryFacade
{
    private readonly IQueryBus _queryBus;

    public SettingQueryFacade(IQueryBus queryBus)
    {
        _queryBus = queryBus;
    }

    public List<SettingViewModel> GetSettings() => _queryBus.Dispatch<List<SettingViewModel>>();
}