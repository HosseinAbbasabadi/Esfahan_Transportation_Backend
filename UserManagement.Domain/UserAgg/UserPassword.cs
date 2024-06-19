using System;
using PhoenixFramework.Domain;

namespace UserManagement.Domain.UserAgg;

public class UserPassword : EntityBase<long>
{
    public Guid Guid { get; private set; }
    public int UserId { get; private set; }
    public string Password { get; private set; }
    public DateTime ExpireDate { get; private set; }

    protected UserPassword()
    {
    }

    public UserPassword(Guid creator, int userId, string password, DateTime expireDate) : base(creator)
    {
        Guid = Guid.NewGuid();
        UserId = userId;
        Password = password;
        ExpireDate = expireDate;
    }

    public bool IsExpired()
    {
        return ExpireDate.Date < DateTime.Now.Date;
    }
}