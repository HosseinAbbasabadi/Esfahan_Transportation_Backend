namespace Phoenix.SSO.Quickstart.Account;

public class CheckAuthorityViewModel : LoginInputModel
{
    public bool HasCaptcha { get; set; }
    public string GeneratedCaptcha { get; set; }
}