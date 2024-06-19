using System;

namespace UserManagement.Query.Contracts.Company;

public class CompanySearchModel
{
    public int IsLocked { get; set; }
    public long ParentId { get; set; }
    public Guid? RootGuid { get; set; }
    public string? Type { get; set; }
}
