using System;
using PhoenixFramework.Application.Command;

namespace UserManagement.Application.Contracts.OrganizationChart;

public class EditOrganizationChart : ICommand
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
    public Guid CompanyGuid { get; set; }
    public string Title { get; set; }
    public Guid ParentGuid { get; set; }
}