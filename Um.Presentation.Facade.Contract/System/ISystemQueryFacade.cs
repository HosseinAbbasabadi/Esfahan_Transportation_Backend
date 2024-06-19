using PhoenixFramework.Company.Query;
using PhoenixFramework.Core;
using UserManagement.Query.Contracts.System;

namespace Um.Presentation.Facade.Contract.System;

public interface ISystemQueryFacade : IFacadeService
{
    List<SystemComboModel> GetForCombo(SystemSearchModel searchModel);
}