using PhoenixFramework.Domain;

namespace UserManagement.Domain.UserAgg;

public class UserSystem : EntityBase<long>
{
    public int UserId { get; private set; }
    public User User { get; private set; }
    public int SystemId { get; private set; }
    public SystemAgg.System System { get; private set; }

    public UserSystem(int userId, int systemId)
    {
        UserId = userId;
        SystemId = systemId;
    }
}