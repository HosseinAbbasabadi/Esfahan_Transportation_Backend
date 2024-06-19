using System;
using System.Collections.Generic;
using System.Linq;
using PhoenixFramework.EntityFramework;
using UserManagement.Domain.RoleAgg;

namespace UserManagement.Persistence.Repository;

public class RoleRepository : BaseRepository<int, Role>, IRoleRepository
{
    private readonly UserManagementCommandContext _context;

    public RoleRepository(UserManagementCommandContext commandContext) : base(commandContext)
    {
        _context = commandContext;
    }

    public bool HasPermission(int roleId, int featureId)
    {
        return _context.Roles
            .SelectMany(x => x.Permissions)
            .Any(x => x.FeatureId == featureId);
    }

    public List<int> GetBatchIdBy(List<Guid> guids) =>
        _context.Roles
            .Where(x => guids.Contains(x.Guid))
            .Select(x => x.Id)
            .ToList();
}