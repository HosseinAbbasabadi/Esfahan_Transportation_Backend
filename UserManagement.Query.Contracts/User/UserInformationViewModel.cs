using System;

namespace UserManagement.Query.Contracts.User;

public class UserInformationViewModel
{
    public bool NeedChangePassword { get; set; }
    public string Fullname { get; set; }
    public string ClassificationLevel { get; set; }
    public Guid ClassificationLevelGuid { get; set; }
    public Guid CompanyGuid { get; set; }
    public string CompanyTitle { get; set; }
    public Guid OrganizationChartGuid { get; set; }
    public string OrganizationChartTitle { get; set; }
}