using PhoenixFramework.Company.Query;
using PhoenixFramework.Core;
using UserManagement.Application.Contracts.Company;
using UserManagement.Query.Contracts.Company;
using UserManagement.Query.Contracts.OrganizationChart;

namespace Um.Presentation.Facade.Contract.Company;

public interface ICompanyQueryFacade : IFacadeService
{
    List<CompanyViewModel> GetList();
    EditCompany GetForEdit(Guid guid);
    List<ComboBase> GetForCombo(CompanySearchModel searchModel);
    List<OrganizationChartViewModel> GetForChart(CompanySearchModel searchModel);
    List<CompanyAdminComboModel> GetForAdminCombo();
}