using System;
using System.Collections.Generic;
using PhoenixFramework.Domain;

namespace UserManagement.Domain.RoleAgg
{
    public interface IRoleRepository : IRepository<int, Role>
    {
        bool HasPermission(int roleId, int featureId);
        List<int> GetBatchIdBy(List<Guid> guids);
    }
}