using PhoenixFramework.Core;
using UserManagement.Query.Contracts.OrganizationChart;

namespace Um.Presentation.Facade.Contract.OrganizationChart;

public interface IOrganizationChartQueryFacade : IFacadeService
{
    List<OrganizationChartTreeViewModel> GetForTree(OrganizationChartSearchModel searchModel);
    List<OrganizationChartViewModel> GetForChart(OrganizationChartSearchModel searchModel);
}