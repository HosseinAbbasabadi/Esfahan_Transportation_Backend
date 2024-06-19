using System;
using System.Collections.Generic;
using System.Data;
using PhoenixFramework.Domain;
using UserManagement.Domain.RoleAgg.Services;
using UserManagement.Domain.UserAgg;

namespace UserManagement.Domain.RoleAgg;

public class Role : AuditableAggregateRootBase<int>
{
    public string Code { get; private set; }
    public int SystemId { get; private set; }
    public string Title { get; private set; }
    public List<RolePermission> Permissions { get; private set; }

    protected Role()
    {
    }

    public Role(Guid creator, string title, string code, List<RolePermission> permissions,
        IRoleValidatorService validatorService) : base(creator)
    {
        GuardAgainstDuplicatedNameAndCode(title,code, validatorService);

        Title = title;
        Code = code;
        Permissions = permissions;
    }

    public void Edit(string title, string code, List<RolePermission> permissions,
        IRoleValidatorService validatorService)
    {
        GuardAgainstDuplicatedNameAndCode(title,code, Id, validatorService);

        Title = title;
        Code = code;
        Permissions = permissions;
    }

    private static void GuardAgainstDuplicatedNameAndCode(string name, string code, IRoleValidatorService validatorService)
    {
        if (validatorService.IsNameAndCodeDuplicated(name,code))
            throw new DuplicateNameException();
    }

    private static void GuardAgainstDuplicatedNameAndCode(string name, string code, int id,
        IRoleValidatorService validatorService)
    {
        if (validatorService.IsNameAndCodeDuplicated(name, code, id))
            throw new DuplicateNameException();
    }
}