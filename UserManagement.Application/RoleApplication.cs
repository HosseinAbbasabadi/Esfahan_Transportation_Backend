using System;
using System.Collections.Generic;
using System.Linq;
using PhoenixFramework.Application.Query;
using PhoenixFramework.Identity;
using UserManagement.Application.Contracts.Commands.Role;
using UserManagement.Application.Contracts.Contracts;
using UserManagement.Application.Contracts.ViewModels;
using UserManagement.Domain.RoleAgg;
using UserManagement.Domain.RoleAgg.Services;
using UserManagement.Query.Contracts.UserManagement.Role;

namespace UserManagement.Application
{
    public class RoleApplication : IRoleApplication
    {
        private readonly IClaimHelper _claimHelper;
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleValidatorService _roleValidatorService;
        private readonly IQueryBus _queryBus;

        public RoleApplication(IRoleRepository roleRepository,
            IRoleValidatorService roleValidatorService, IClaimHelper claimHelper, IQueryBus queryBus)
        {
            _roleRepository = roleRepository;
            _roleValidatorService = roleValidatorService;
            _claimHelper = claimHelper;
            _queryBus = queryBus;
        }

        public void CreateRole(CreateRole command)
        {
            var creator = _claimHelper.GetCurrentUserGuid();
            var permissions = ProducePermissions(command.Permissions);

            var role = new Role(creator, command.Title, command.Code, permissions, _roleValidatorService);
      
            _roleRepository.Create(role);
            _roleRepository.SaveChanges();
        }

        private static List<RolePermission> ProducePermissions(IEnumerable<int> permissions)
        {
            return permissions.Select(permission => new RolePermission(permission))
                .ToList();
        }

        public void EditRole(EditRole command)
        {
            var role = _roleRepository.Load(command.Guid, "Permissions");
            var permissions = ProducePermissions(command.Permissions);
            role.Edit(command.Title, command.Code, permissions, _roleValidatorService);
            _roleRepository.SaveChanges();
        }

        public List<RoleViewModel> List()
        {
            return _queryBus.Dispatch<List<RoleViewModel>>();
        }

        public EditRole GetDetails(Guid guid)
        {
            return _queryBus.Dispatch<EditRole, Guid>(guid);
        }

        public List<RoleComboModel> GetForCombo()
        {
            return _queryBus.Dispatch<List<RoleComboModel>>();
        }

        public void Delete(Guid id)
        {
            var role = _roleRepository.Load(id);

            _roleRepository.Delete(role);
            _roleRepository.SaveChanges();
        }
    }
}