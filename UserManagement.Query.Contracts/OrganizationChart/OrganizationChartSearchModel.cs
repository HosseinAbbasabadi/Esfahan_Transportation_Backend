using System;

namespace UserManagement.Query.Contracts.OrganizationChart;

public class OrganizationChartSearchModel
{
    public Guid RootGuid { get; set; }
    public Guid? OrganizationChartGuid { get; set; }
}