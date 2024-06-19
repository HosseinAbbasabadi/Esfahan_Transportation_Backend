using System;
using PhoenixFramework.Application.Command;

namespace UserManagement.Application.Contracts.Company;

public class DeleteCompany : ICommand
{
    public Guid Guid { get; set; }
 
    public DeleteCompany(Guid guid)
    {
        Guid = guid;
    }
}