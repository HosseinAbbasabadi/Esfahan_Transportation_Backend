using System;
using PhoenixFramework.Application.Command;

namespace UserManagement.Application.Contracts.OrganizationChart;

public class DeleteOrganizationChart : ICommand
{
    public Guid Guid { get; set; }

    public DeleteOrganizationChart(Guid guid)
    {
        Guid = guid;
    }
}