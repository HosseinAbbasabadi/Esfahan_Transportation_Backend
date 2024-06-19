using System;
using System.Collections.Generic;
using System.Linq;
using PhoenixFramework.Application.Query;
using PhoenixFramework.Dapper;
using UserManagement.Persistence;
using UserManagement.Query.Contracts.OrganizationChart;

namespace UserManagement.Query;

public class OrganizationChartQueryHandler :
    IQueryHandler<List<OrganizationChartTreeViewModel>, OrganizationChartSearchModel>,
    IQueryHandler<List<OrganizationChartViewModel>, OrganizationChartSearchModel>
{
    private const string OrganizationChartSpName = "spGetOrganizationChartFor";
    private readonly UserManagementQueryContext _context;
    private readonly BaseDapperRepository _repository;

    public OrganizationChartQueryHandler(BaseDapperRepository repository, UserManagementQueryContext context)
    {
        _repository = repository;
        _context = context;
    }

    public List<OrganizationChartTreeViewModel> Handle(OrganizationChartSearchModel searchModel)
    {
        var result = _repository.SelectFromSp<OrganizationChartTreeViewModel>(OrganizationChartSpName,
            new { Output = "Tree", searchModel.RootGuid });

        if (searchModel.OrganizationChartGuid is null) return result;
        var parent = result
            .First(x => x.Guid == searchModel.OrganizationChartGuid);

        var parentCode = parent.Id.ToString();
        if (parent.ParentCode is not null && parent.ParentCode != "0")
            parentCode = $"{parent.ParentCode}_{parent.Id}";

        result = result
            //.Where(x => x.ParentCode is not null)
            .Where(x => x.ParentCode.StartsWith(parentCode) || x.Guid == searchModel.OrganizationChartGuid)
            .ToList();

        var company = _context.Companies.First(x => x.Guid == searchModel.RootGuid);
        result.First(x => x.Guid == searchModel.OrganizationChartGuid).ParentId = company.Id;

        return result;
    }

    List<OrganizationChartViewModel> IQueryHandler<List<OrganizationChartViewModel>, OrganizationChartSearchModel>.
        Handle(OrganizationChartSearchModel searchModel) =>
        _repository.SelectFromSp<OrganizationChartViewModel>(OrganizationChartSpName,
            new { Output = "Chart", searchModel.RootGuid });
}