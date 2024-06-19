using PhoenixFramework.Application.Query;
using Um.Presentation.Facade.Contract.OrganizationChart;
using UserManagement.Query.Contracts.OrganizationChart;

namespace Um.Presentation.Facade.Query;

public class OrganizationChartQueryFacade : IOrganizationChartQueryFacade
{
    private readonly IQueryBus _queryBus;

    public OrganizationChartQueryFacade(IQueryBus queryBus)
    {
        _queryBus = queryBus;
    }

    public List<OrganizationChartTreeViewModel> GetForTree(OrganizationChartSearchModel searchModel)
    {
        return _queryBus.Dispatch<List<OrganizationChartTreeViewModel>, OrganizationChartSearchModel>(searchModel);
    }

    public List<OrganizationChartViewModel> GetForChart(OrganizationChartSearchModel searchModel)
    {
        return _queryBus.Dispatch<List<OrganizationChartViewModel>, OrganizationChartSearchModel>(searchModel);
    }
}