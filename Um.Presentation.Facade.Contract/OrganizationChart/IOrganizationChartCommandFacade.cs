using PhoenixFramework.Core;
using UserManagement.Application.Contracts.OrganizationChart;

namespace Um.Presentation.Facade.Contract.OrganizationChart;

public interface IOrganizationChartCommandFacade : IFacadeService
{
    void Create(ChangeOrganizationChartParent command);
    long Edit(EditOrganizationChart command);
    void Delete(Guid guid);
}