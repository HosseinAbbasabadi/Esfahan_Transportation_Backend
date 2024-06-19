using System;
using System.Collections.Generic;
using System.Linq;
using PhoenixFramework.Application.Query;
using PhoenixFramework.Company.Query;
using PhoenixFramework.Dapper;
using PhoenixFramework.Identity;
using UserManagement.Application.Contracts.Company;
using UserManagement.Persistence;
using UserManagement.Query.Contracts.Company;
using UserManagement.Query.Contracts.OrganizationChart;

namespace UserManagement.Query;

public class CompanyQueryHandler :
    IQueryHandler<List<CompanyViewModel>>,
    IQueryHandler<EditCompany, Guid>,
    IQueryHandler<List<ComboBase>, CompanySearchModel>,
    IQueryHandler<List<OrganizationChartViewModel>, CompanySearchModel>,
    IQueryHandler<List<CompanyAdminComboModel>>
{
    private const string CompanySpName = "spGetCompanyFor";
    private readonly BaseDapperRepository _dapper;
    private readonly IClaimHelper _claimHelper;
    private readonly UserManagementQueryContext _context;

    public CompanyQueryHandler(BaseDapperRepository dapper, IClaimHelper claimHelper,
        UserManagementQueryContext context)
    {
        _dapper = dapper;
        _claimHelper = claimHelper;
        _context = context;
    }

    public List<CompanyViewModel> Handle()
    {
        return _dapper.SelectFromSp<CompanyViewModel>(CompanySpName, new { Output = QueryOutputs.List });
    }

    public EditCompany Handle(Guid guid)
    {
        return _dapper.SelectFromSpFirstOrDefault<EditCompany>(CompanySpName,
            new { Output = QueryOutputs.Edit, Guid = guid });
    }

    public List<ComboBase> Handle(CompanySearchModel searchModel)
    {
        var currentUserGuid = _claimHelper.GetCurrentUserGuid();
        return _dapper.SelectFromSp<ComboBase>(CompanySpName,
            new { Output = QueryOutputs.Combo, searchModel.IsLocked, UserGuid = currentUserGuid });
    }

    List<OrganizationChartViewModel> IQueryHandler<List<OrganizationChartViewModel>, CompanySearchModel>.Handle(
        CompanySearchModel searchModel)
    {
        var output = "Chart";
        if (!string.IsNullOrWhiteSpace(searchModel.Type))
            output = "HoldingChart";

        return _dapper.SelectFromSp<OrganizationChartViewModel>(CompanySpName,
            new { Output = output, searchModel.RootGuid });
    }

    List<CompanyAdminComboModel> IQueryHandler<List<CompanyAdminComboModel>>.Handle()
    {
        return _dapper.SelectFromSp<CompanyAdminComboModel>(CompanySpName,
            new { Output = QueryOutputs.ComboAll });
    }
}