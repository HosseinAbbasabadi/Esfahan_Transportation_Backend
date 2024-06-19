using System.Collections.Generic;

namespace UserManagement.Application.Contracts.Commands.Role
{
    public class CreateRole
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public List<int> Permissions { get; set; }
    }
}