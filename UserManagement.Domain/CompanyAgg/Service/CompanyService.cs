using System;
using System.Linq.Expressions;

namespace UserManagement.Domain.CompanyAgg.Service;

public class CompanyService : ICompanyService
{
    private readonly Expression<Func<Company, bool>> Predicate;
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public void CheckIfCompanyCanBeDeleted(Company company)
    {
    }

    public string? GenerateParentCode(long? parentId)
    {
        if (parentId is 0) return null;

        var parentCode = _companyRepository.Load(parentId.Value).ParentCode;
        
        if (parentCode is null)
            return parentId.Value.ToString();

        return $"{parentCode}_{parentId}";
    }
}