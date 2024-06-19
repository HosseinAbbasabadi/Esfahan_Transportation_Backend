using System;
using System.Collections.Generic;
using PhoenixFramework.Application.Command;
using PhoenixFramework.Core.Validation;

namespace UserManagement.Application.Contracts.Commands.User
{
    public class CreateUser : ICommand
    {
        public int Id { get; set; }
        public List<Guid> RoleGuids { get; set; }
        public List<Guid> SystemGuids { get; set; }
        public string Username { get; set; }
        [NationalCode] public string NationalCode { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string Mobile { get; set; }
        public string? Email { get; set; }
        public string Fullname { get; set; }
        public string EmployeeCode { get; set; }
        public Guid ClassificationLevelGuid { get; set; }
        public Guid CompanyGuid { get; set; }
        public Guid OrganizationChartGuid { get; set; }
    }
}