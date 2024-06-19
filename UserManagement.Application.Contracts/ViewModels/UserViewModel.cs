using System;
using PhoenixFramework.Company.Query;

namespace UserManagement.Application.Contracts.ViewModels;

public class UserViewModel : ViewModelAbilities
{
    public long RoleId { get; set; }
    public string RoleName { get; set; }
    public string Username { get; set; }
    public string Mobile { get; set; }
    public string Fullname { get; set; }
    public string CompanyTitle { get; set; }
    public string OrganizationChartTitle { get; set; }
    public string EmployeeCode { get; set; }
}