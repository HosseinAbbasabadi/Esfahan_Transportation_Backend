using PhoenixFramework.Company.Query;
using System;

namespace UserManagement.Query.Contracts.OrganizationChart;

public class OrganizationChartTreeViewModel : ViewModelAbilities
{
    public string? Title { get; set; }
    public long? ParentId { get; set; }
    public string ParentCode { get; set; }
    public Guid CompanyGuid { get; set; }
}