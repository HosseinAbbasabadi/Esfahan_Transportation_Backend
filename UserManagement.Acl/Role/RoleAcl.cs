using UserManagement.Domain.Feature;
using UserManagement.Domain.RoleAgg;

namespace UserManagement.Acl.Role
{
    public class RoleAcl : IRoleAcl
    {
        private readonly IFeatureRepository _featureRepository;
        private readonly IRoleRepository _roleRepository;

        public RoleAcl(IFeatureRepository featureRepository, IRoleRepository roleRepository)
        {
            _featureRepository = featureRepository;
            _roleRepository = roleRepository;
        }

        public bool CheckUserHasPermission(int roleId, string permission)
        {
            var featureId = _featureRepository.GetIdBy(permission);

            if (featureId == 0) return false;

            return _roleRepository.HasPermission(roleId, featureId);
        }
    }
}