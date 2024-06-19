using PhoenixFramework.Core;

namespace UserManagement.Domain.OrganizationChartAgg.Services;

public interface IOrganizationChartService : IDomainService
{
    void ThrowWhenNodeIsDuplicated(string title, long parentId);
    string? GenerateParentCode(long? parentId);
}