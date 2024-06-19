using System.Linq;
using PhoenixFramework.Dapper;
using PhoenixFramework.Identity;
using UserManagement.Persistence;
using System.Collections.Generic;
using PhoenixFramework.Application.Query;
using UserManagement.Query.Contracts.System;

namespace UserManagement.Query;

public class SystemQueryHandler : IQueryHandler<List<SystemComboModel>, SystemSearchModel>
{
    private const string RoleSpName = "spGetRoleFor";
    private readonly IClaimHelper _claimHelper;
    private readonly BaseDapperRepository _repository;
    private readonly UserManagementQueryContext _context;

    public SystemQueryHandler(IClaimHelper claimHelper, BaseDapperRepository repository,
        UserManagementQueryContext context)
    {
        _claimHelper = claimHelper;
        _repository = repository;
        _context = context;
    }

    public List<SystemComboModel> Handle(SystemSearchModel condition) =>
        _context.Systems
            .Where(x => x.IsActive == 1)
            .Select(x => new SystemComboModel
            {
                Guid = x.Guid,
                Title = x.Title
            }).ToList();
}