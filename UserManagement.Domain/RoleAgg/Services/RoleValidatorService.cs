namespace UserManagement.Domain.RoleAgg.Services;

public class RoleValidatorService : IRoleValidatorService
{
    private readonly IRoleRepository _roleRepository;

    public RoleValidatorService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public bool IsNameAndCodeDuplicated(string name, string code)
    {
        return _roleRepository.Exists(x => x.Title == name || x.Code==code);
    }

    public bool IsNameAndCodeDuplicated(string name, string code, int id)
    {
        return _roleRepository.Exists(x => (x.Title == name || x.Code == code) && x.Id != id);
    }
}