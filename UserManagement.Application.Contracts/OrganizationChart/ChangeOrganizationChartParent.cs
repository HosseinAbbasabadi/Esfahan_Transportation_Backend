using System;
using PhoenixFramework.Application.Command;

namespace UserManagement.Application.Contracts.OrganizationChart;

public class ChangeOrganizationChartParent : ICommand
{
    public Guid Guid { get; set; }
    public Guid ParentGuid { get; set; }
}