using System;
using System.Collections.Generic;
using PhoenixFramework.Domain;

namespace UserManagement.Domain.SystemAgg;

public interface ISystemRepository : IRepository<int, System>
{
    List<int> GetBatchIdBy(List<Guid> commandSystemGuids);
}