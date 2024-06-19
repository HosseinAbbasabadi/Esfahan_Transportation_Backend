using System;
using PhoenixFramework.Domain;
using UserManagement.Domain.CompanyAgg.Service;

namespace UserManagement.Domain.CompanyAgg;

public class Company : AuditableAggregateRootBase<long>
{
    public int SystemId { get; private set; }
    public string Title { get; private set; }
    public long? ParentId { get; private set; }
    public string? ParentCode { get; private set; }
    public string? Address { get; private set; }
    public byte[]? Logo { get; private set; }
    public string? Description { get; private set; }

    protected Company()
    {
    }

    public Company(Guid creator, string title, long? parentId, string? address, 
        byte[]? logo, string? description, ICompanyService service) : base(creator)
    {
        Title = title;
        ParentId = parentId;
        ParentCode = service.GenerateParentCode(parentId??0);
        Address = address;
        Logo = logo;
        Description = description;
    }

    public void Edit(Guid editor, string title, long? parentId, string? address, 
        byte[]? logo, string? description, ICompanyService service)
    {
        Title = title;
        ParentId = parentId;
        ParentCode = service.GenerateParentCode(parentId??0);
        Address = address;
        Description = description;

        if (logo is not null)
            Logo = logo;

        Modified(editor);
    }
}