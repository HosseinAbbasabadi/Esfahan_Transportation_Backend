using PhoenixFramework.Application.Query;
using PhoenixFramework.Company.Query;
using Um.Presentation.Facade.Contract.Company;
using UserManagement.Application.Contracts.Company;
using UserManagement.Query.Contracts.Company;
using UserManagement.Query.Contracts.OrganizationChart;

namespace Um.Presentation.Facade.Query;

public class CompanyQueryFacade : ICompanyQueryFacade
{
    private readonly IQueryBus _queryBus;

    public CompanyQueryFacade(IQueryBus queryBus)
    {
        _queryBus = queryBus;
    }

    public List<ComboBase> GetForCombo(CompanySearchModel searchModel)
    {
        return _queryBus.Dispatch<List<ComboBase>, CompanySearchModel>(searchModel);
    }

    public List<OrganizationChartViewModel> GetForChart(CompanySearchModel searchModel)
    {
        return _queryBus.Dispatch<List<OrganizationChartViewModel>, CompanySearchModel>(searchModel);
    }

    public List<CompanyAdminComboModel> GetForAdminCombo()
    {
        return _queryBus.Dispatch<List<CompanyAdminComboModel>>();
    }

    public EditCompany GetForEdit(Guid guid)
    {
        return _queryBus.Dispatch<EditCompany, Guid>(guid);
    }

    public List<CompanyViewModel> GetList()
    {
        return _queryBus.Dispatch<List<CompanyViewModel>>();
    }

    public EditCompany GetByGuid(Guid guid)
    {
        return _queryBus.Dispatch<EditCompany, Guid>(guid);
    }
}