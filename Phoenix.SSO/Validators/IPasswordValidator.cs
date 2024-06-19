namespace Phoenix.SSO.Validators;

public interface IPasswordValidator
{
    PasswordValidationResult Validate(string username, string password);
}