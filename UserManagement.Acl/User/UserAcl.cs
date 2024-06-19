using System;
using UserManagement.Domain.UserAgg;

namespace UserManagement.Acl.User;

public class UserAcl : IUserAcl
{
    private readonly IUserRepository _userRepository;

    public UserAcl(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public int GetIdBy(Guid guid)
    {
        return _userRepository.GetIdBy(guid);
    }
}