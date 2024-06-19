using PhoenixFramework.Application.Query;
using UserManagement.Query.Contracts.System;
using Um.Presentation.Facade.Contract.System;

namespace Um.Presentation.Facade.Query;

public class SystemQueryFacade : ISystemQueryFacade
{
    private readonly IQueryBus _queryBus;

    public SystemQueryFacade(IQueryBus queryBus)
    {
        _queryBus = queryBus;
    }

    public List<SystemComboModel> GetForCombo(SystemSearchModel searchModel) =>
        _queryBus.Dispatch<List<SystemComboModel>, SystemSearchModel>(searchModel);
}