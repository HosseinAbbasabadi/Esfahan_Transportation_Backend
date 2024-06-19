using PhoenixFramework.Core;

namespace UserManagement.Domain.RoleAgg.Services
{
    public interface IRoleValidatorService : IDomainService
    {
        bool IsNameAndCodeDuplicated(string name, string code);
        bool IsNameAndCodeDuplicated(string name, string code, int id);
    }
}
