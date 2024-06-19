using System;

namespace UserManagement.Application.Contracts.Commands.User
{
    public class EditUser : CreateUser
    {
        public Guid Guid { get; set; }
        public string OrganizationChartTitle { get; set; }
    }
}