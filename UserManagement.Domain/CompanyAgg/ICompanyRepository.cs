using System;
using PhoenixFramework.Domain;

namespace UserManagement.Domain.CompanyAgg;

public interface ICompanyRepository : IRepository<long, Company>
{
    long GetIdBy(Guid guid);
}