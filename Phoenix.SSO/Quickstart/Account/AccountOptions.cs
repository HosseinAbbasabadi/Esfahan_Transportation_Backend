using System;

namespace Phoenix.SSO.Quickstart.Account;

public static class AccountOptions
{
    public const bool AllowLocalLogin = true;
    public const bool AllowRememberLogin = false;
    public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

    public const bool ShowLogoutPrompt = true;
    public const bool AutomaticRedirectAfterSignOut = false;

    public const string InvalidCredentialsErrorMessage = "نام کاربری یا کلمه رمز اشتباه است";
    public const string InvalidCaptchaErrorMessage = "عبارت اعتبارسنجی اشتباه است لطفا مجددا تلاش نمایید.";
}