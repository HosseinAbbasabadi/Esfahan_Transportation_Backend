using PhoenixFramework.Application.Setting;

namespace UserManagement.Query.Contracts.Setting;

public class UserManagementSettingViewModel : ISetting
{
    [SettingName("NumberOfUserSessionsToShow")]
    public int NumberOfUserSessionsToShow { get; set; }
}