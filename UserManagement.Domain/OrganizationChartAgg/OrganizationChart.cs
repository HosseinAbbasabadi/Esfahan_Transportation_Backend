using System;
using PhoenixFramework.Domain;
using UserManagement.Domain.OrganizationChartAgg.Services;

namespace UserManagement.Domain.OrganizationChartAgg;

public class OrganizationChart : AuditableAggregateRootBase<long>
{
    public long CompanyId { get; private set; }
    public string Title { get; private set; }
    public long ParentId { get; private set; }
    public string? ParentCode { get; private set; }

    protected OrganizationChart()
    {
    }

    public OrganizationChart(Guid creator, Guid guid, long companyId, string title, long parentId,
        IOrganizationChartService service) : base(creator, guid)
    {
        service.ThrowWhenNodeIsDuplicated(title, parentId);

        CompanyId = companyId;
        Title = title;
        ParentId = parentId;
        ParentCode = service.GenerateParentCode(parentId);
    }

    public void Edit(Guid actor, string title)
    {
        Title = title;
        Modified(actor);
    }

    public void ChangeParent(Guid actor, long parentId)
    {
        ParentId = parentId;
        Modified(actor);
    }
}