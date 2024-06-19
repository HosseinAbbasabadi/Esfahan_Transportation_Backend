namespace Phoenix.SSO.SettingModels
{
    public class ChangePassword
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string CurrentPassword { get; set; }
    }
}
