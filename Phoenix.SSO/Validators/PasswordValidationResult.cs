namespace Phoenix.SSO.Validators;

public class PasswordValidationResult
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }

    public PasswordValidationResult Failed(string message)
    {
        Succeeded = false;
        Message = message;
        return this;
    }

    public PasswordValidationResult Success()
    {
        Succeeded = true;
        return this;
    }
}