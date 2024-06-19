using System;
using PhoenixFramework.Domain;
using UserManagement.Domain.RoleAgg;

namespace UserManagement.Domain.UserAgg;

public class UserRole : EntityBase<long>
{
    public int UserId { get; private set; }
    public User User { get; private set; }
    public int RoleId { get; private set; }

    public UserRole(int userId, int roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
}