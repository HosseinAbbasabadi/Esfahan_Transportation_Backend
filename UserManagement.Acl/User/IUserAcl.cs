using System;

namespace UserManagement.Acl.User
{
    public interface IUserAcl
    {
        int GetIdBy(Guid guid);
    }
}
