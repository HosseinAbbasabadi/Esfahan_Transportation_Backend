using System;
using System.Linq;
using PhoenixFramework.EntityFramework;
using UserManagement.Domain.UserAgg;

namespace UserManagement.Persistence.Repository;

public class UserRepository : BaseRepository<int, User>, IUserRepository
{
    private readonly UserManagementCommandContext _context;

    public UserRepository(UserManagementCommandContext commandContext) : base(commandContext)
    {
        _context = commandContext;
    }

    public User GetByUsername(string username)
    {
        return _context.Users
            .FirstOrDefault(x => x.Username == username);
    }

    public long GetIdBy(Guid guid)
    {
        return _context.Users
            .FirstOrDefault(x => x.Guid == guid).Id
            ;
    }
}