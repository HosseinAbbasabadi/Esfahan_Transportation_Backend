using PhoenixFramework.Core.Exceptions;

namespace UserManagement.Domain.OrganizationChartAgg.Services;

public class OrganizationChartService : IOrganizationChartService
{
    private readonly IOrganizationChartRepository _organizationChartRepository;

    public OrganizationChartService(IOrganizationChartRepository organizationChartRepository)
    {
        _organizationChartRepository = organizationChartRepository;
    }

    public void ThrowWhenNodeIsDuplicated(string title, long parentId)
    {
        if (_organizationChartRepository.Exists(x => x.Title == title && x.ParentId == parentId))
            throw new BusinessException("0", "واحد سازمانی با این نام قبلا ثبت شده است.");
    }

    public string GenerateParentCode(long? parentId)
    {
        if (parentId is 0) return null;

        var parentCode = _organizationChartRepository.Load(parentId.Value).ParentCode;

        if (parentCode is null)
            return parentId.Value.ToString();

        return $"{parentCode}_{parentId}";
    }
}