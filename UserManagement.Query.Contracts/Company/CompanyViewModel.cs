using PhoenixFramework.Company.Query;
using System;

namespace UserManagement.Query.Contracts.Company;

public class CompanyViewModel : ViewModelAbilities
{
    public string? Title { get; set; }
    public long? ParentId { get; set; }
    public Guid? ParentGuid { get; set; }
    public string? ParentTitle { get; set; }
    public string? Address { get; set; }
    public byte[]? Logo { get; set; }
    public string? Description { get; set; }

    public int CountUsed { get; set; }
}