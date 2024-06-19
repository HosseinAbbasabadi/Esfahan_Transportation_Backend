using PhoenixFramework.Application.Setting;

namespace UserManagement.Application.Contracts.ViewModels;

public class SecuritySettingViewModel : ISetting
{
    [SettingName("TokenExpiryTime")]
    public int TokenExpiryTime { get; set; }
    
    [SettingName("LoginAttemptsCountLimit")]
    public int LoginAttemptsCountLimit { get; set; }
    
    [SettingName("PasswordLifetimeDays")]
    public int PasswordLifetimeDays { get; set; }
    
    [SettingName("ForbiddenOldPasswordsCount")]
    public int ForbiddenOldPasswordsCount { get; set; }
}