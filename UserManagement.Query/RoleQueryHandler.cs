using System.Linq;
using System.Collections.Generic;
using PhoenixFramework.Application.Query;
using PhoenixFramework.Dapper;
using PhoenixFramework.Identity;
using System;
using UserManagement.Application.Contracts.Commands.Role;
using UserManagement.Persistence;
using UserManagement.Query.Contracts.UserManagement.Role;

namespace UserManagement.Query
{
    public class RoleQueryHandler :
        IQueryHandler<string>,
        IQueryHandler<EditRole, Guid>,
        IQueryHandler<List<RoleViewModel>>,
        IQueryHandler<List<RoleComboModel>>
    {
        private const string RoleSpName = "spGetRoleFor";
        private readonly IClaimHelper _claimHelper;
        private readonly BaseDapperRepository _repository;
        private readonly UserManagementQueryContext _context;

        public RoleQueryHandler(IClaimHelper claimHelper, BaseDapperRepository repository,
            UserManagementQueryContext context)
        {
            _claimHelper = claimHelper;
            _repository = repository;
            _context = context;
        }

        public string Handle()
        {
            var userGuid = _claimHelper.GetCurrentUserGuid();

            var permissions = (from feature in _context.Features
                join rolePermission in _context.RolesPermissions on
                    feature.Id equals rolePermission.FeatureId
                join role in _context.Roles on
                    rolePermission.RoleId equals role.Id
                join userRole in _context.UserRoles on
                    role.Id equals userRole.RoleId
                join user in _context.Users on
                    userRole.UserId equals user.Id
                where user.Guid == userGuid
                group feature by feature.Title
                into res
                select res.Key).ToList();

            return string.Join(",", permissions);
        }

        List<RoleViewModel> IQueryHandler<List<RoleViewModel>>.Handle()
        {
            return _repository.SelectFromSp<RoleViewModel>(RoleSpName, new { Type = QueryOutputs.List });
        }

        public EditRole Handle(Guid guid)
        {
            return _repository.SelectFromSpFirstOrDefault<EditRole>(RoleSpName,
                new { Type = QueryOutputs.Edit, guid });
        }

        List<RoleComboModel> IQueryHandler<List<RoleComboModel>>.Handle()
        {
            return _repository.SelectFromSp<RoleComboModel>(RoleSpName, new { Type = QueryOutputs.Combo });
        }
    }
}