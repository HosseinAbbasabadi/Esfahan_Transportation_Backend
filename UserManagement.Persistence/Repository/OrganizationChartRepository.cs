using System;
using System.Linq;
using PhoenixFramework.EntityFramework;
using UserManagement.Domain.OrganizationChartAgg;

namespace UserManagement.Persistence.Repository;

public class OrganizationChartRepository : BaseRepository<long, OrganizationChart>, IOrganizationChartRepository
{
    private readonly UserManagementCommandContext _context;

    public OrganizationChartRepository(UserManagementCommandContext commandContext) : base(commandContext)
    {
        _context = commandContext;
    }

    public long GetIdBy(Guid guid)
    {
        return _context.OrganizationCharts
            .Where(x => x.Guid == guid)
            .Select(x => x.Id)
            .FirstOrDefault();
    }
}