using System;
using System.Collections.Generic;
using System.Linq;
using PhoenixFramework.EntityFramework;
using UserManagement.Domain.SystemAgg;

namespace UserManagement.Persistence.Repository;

public class SystemRepository : BaseRepository<int, Domain.SystemAgg.System>, ISystemRepository
{
    private readonly UserManagementCommandContext _context;

    public SystemRepository(UserManagementCommandContext context) : base(context)
    {
        _context = context;
    }

    public List<int> GetBatchIdBy(List<Guid> guids) =>
        _context.Systems
            .Where(x => guids.Contains(x.Guid))
            .Select(x => x.Id)
            .ToList();
}