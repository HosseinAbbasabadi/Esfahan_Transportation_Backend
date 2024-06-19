using System;

namespace UserManagement.Application.Contracts.Commands.Role
{
    public class EditRole : CreateRole
    {
        public Guid Guid { get; set; }
        public int IsLocked { get; set; }
    }
}