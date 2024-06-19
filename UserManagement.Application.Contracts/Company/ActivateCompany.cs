using System;

namespace UserManagement.Application.Contracts.Company;

public class ActivateCompany : CreateCompany
{
    public ActivateCompany(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
