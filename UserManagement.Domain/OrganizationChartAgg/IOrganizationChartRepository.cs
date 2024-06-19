using System;
using PhoenixFramework.Domain;

namespace UserManagement.Domain.OrganizationChartAgg;

public interface IOrganizationChartRepository : IRepository<long, OrganizationChart>
{
    long GetIdBy(Guid guid);
}