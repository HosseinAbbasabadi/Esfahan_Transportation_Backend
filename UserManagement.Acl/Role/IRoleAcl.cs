namespace UserManagement.Acl.Role
{
    public interface IRoleAcl
    {
        bool CheckUserHasPermission(int roleId, string permission);
    }
}