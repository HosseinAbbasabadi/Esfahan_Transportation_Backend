using PhoenixFramework.Domain;
using System;

namespace UserManagement.Domain.UserAgg;

public interface IUserRepository : IRepository<int, User>
{
    User GetByUsername(string username);
    int GetIdBy(Guid guid);
}