using PhoenixFramework.Core;

namespace UserManagement.Domain.CompanyAgg.Service;

public interface ICompanyService : IDomainService
{
    void CheckIfCompanyCanBeDeleted(Company company);
    string? GenerateParentCode(long? parentId);
}
