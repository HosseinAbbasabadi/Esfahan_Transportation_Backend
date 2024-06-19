using System;

namespace UserManagement.Application.Contracts.Company;

public class DeactivateCompany : CreateCompany
{
    public DeactivateCompany(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
