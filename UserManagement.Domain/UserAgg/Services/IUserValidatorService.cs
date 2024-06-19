using PhoenixFramework.Core;

namespace UserManagement.Domain.UserAgg.Services
{
    public interface IUserValidatorService : IDomainService
    {
        void CheckUserExistence(string username);
    }
}
