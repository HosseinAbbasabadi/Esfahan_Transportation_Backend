using PhoenixFramework.Domain;

namespace UserManagement.Domain.RoleAgg;

public class RolePermission : EntityBase<int>
{
    public int FeatureId { get; private set; }
    public int RoleId { get; private set; }
    public Role Role { get; private set; }

    protected RolePermission()
    {
    }

    public RolePermission(int featureId)
    {
        FeatureId = featureId;
    }
}