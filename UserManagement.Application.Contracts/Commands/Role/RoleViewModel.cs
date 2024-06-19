using PhoenixFramework.Company.Query;

namespace UserManagement.Application.Contracts.Commands.Role
{
    public class RoleViewModel : ViewModelAbilities
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public int UserCount { get; set; }
        public int IsLocked { get; set; }
    }
}