using System;
using System.Linq;
using PhoenixFramework.EntityFramework;
using UserManagement.Domain.CompanyAgg;

namespace UserManagement.Persistence.Repository;

public class CompanyRepository : BaseRepository<long, Company>, ICompanyRepository
{
    private readonly UserManagementCommandContext _context;

    public CompanyRepository(UserManagementCommandContext commandContext) : base(commandContext)
    {
        _context = commandContext;
    }

    public long GetIdBy(Guid guid)
    {
        return _context.Companies
            .Where(x => x.Guid == guid)
            .Select(x => x.Id)
            .SingleOrDefault();
    }
}