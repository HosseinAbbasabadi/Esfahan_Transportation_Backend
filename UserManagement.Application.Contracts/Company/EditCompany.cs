using System;

namespace UserManagement.Application.Contracts.Company;

public class EditCompany : CreateCompany
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
}
