using System;

namespace UserManagement.Query.Contracts.User;

public class UserSessionViewModel
{
    public Guid Guid { get; set; }
    public Guid UserGuid { get; set; }
    public bool IsSuccessful { get; set; }
    public string IsSuccessfulTitle { get; set; }
    public string ClientIpAddress { get; set; }
    public DateTime CreatedEng { get; set; }
    public string Created { get; set; }
    public string? Fullname { get; set; }
    public string? Username { get; set; }
    public Guid? CompanyGuid { get; set; }
    public string? CompanyTitle { get; set; }
    public Guid? OrganizationChartGuid { get; set; }
    public string? OrganizationChartTitle { get; set; }
    public string NationalCode { get; set; }
}