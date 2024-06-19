using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Test;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Phoenix.SSO.SettingModels;
using Phoenix.SSO.Validators;
using PhoenixFramework.Application.Setting;
using PhoenixFramework.Identity;
using UserManagement.Persistence;

namespace Phoenix.SSO.Quickstart.Account;

[SecurityHeaders]
[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IClientStore _clientStore;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IEventService _events;
    private readonly IPasswordValidator _passwordValidator;
    private readonly UserManagementCommandContext _context;
    private readonly ISettingService _settingService;
    private readonly IPasswordHasher _passwordHasher;

    public AccountController(IIdentityServerInteractionService interaction, IClientStore clientStore,
        IAuthenticationSchemeProvider schemeProvider, IEventService events, IPasswordValidator passwordValidator,
        UserManagementCommandContext context, ISettingService settingService, IPasswordHasher passwordHasher)
    {
        _interaction = interaction;
        _clientStore = clientStore;
        _schemeProvider = schemeProvider;
        _events = events;
        _passwordValidator = passwordValidator;
        _context = context;
        _settingService = settingService;
        _passwordHasher = passwordHasher;
    }

    public string MakeRandom(int lengthOfCode = 6)
    {
        const string possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var random = new Random();
        return new string(Enumerable.Repeat(possible, lengthOfCode)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl)
    {
        var viewModel = await BuildLoginViewModelAsync(returnUrl);

        if (viewModel.IsExternalLoginOnly)
            return RedirectToAction("Challenge", "External", new { scheme = viewModel.ExternalLoginScheme, returnUrl });

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginInputModel model, string button)
    {
        var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

        if (string.IsNullOrWhiteSpace(model.EnteredCaptcha) || !string.Equals(model.EnteredCaptcha,
                model.GeneratedCaptcha, StringComparison.CurrentCultureIgnoreCase))
        {
            await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials",
                clientId: context?.Client.ClientId));
            ModelState.AddModelError(string.Empty, AccountOptions.InvalidCaptchaErrorMessage);
        }

        if (button != "login")
        {
            if (context == null) return Redirect("~/");
            await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

            return context.IsNativeClient() ? this.LoadingPage("Redirect", model.ReturnUrl) : Redirect(model.ReturnUrl);
        }

        if (ModelState.IsValid)
        {
            var validationResult = _passwordValidator.Validate(model.Username, model.Password);
            if (validationResult.Succeeded)
            {
                var user = _context.Users.First(x => x.Username == model.Username);
                await _events.RaiseAsync(new UserLoginSuccessEvent(
                    user.Username,
                    user.Guid.ToString(),
                    user.Username,
                    clientId: context?.Client.ClientId));

                AuthenticationProperties props = null;
                if (AccountOptions.AllowRememberLogin && model.RememberLogin)
                {
                    props = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                    };
                }

                ;

                var issuer = new IdentityServerUser(user.Guid.ToString())
                {
                    DisplayName = user.Username
                };

                await HttpContext.SignInAsync(issuer, props);

                // if (context != null)
                // {
                //     if (context.IsNativeClient())
                //         return this.LoadingPage("Redirect", model.ReturnUrl);
                //
                //     return Redirect(model.ReturnUrl);
                // }
                //
                // if (Url.IsLocalUrl(model.ReturnUrl))
                // {
                //     return Redirect(model.ReturnUrl);
                // }
                //
                // if (string.IsNullOrEmpty(model.ReturnUrl))
                // {
                //     return RedirectToAction("Index", "Grants");
                // }
                //
                return RedirectToAction("Index", "Grants");

                throw new Exception("invalid return URL");
            }

            await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, validationResult.Message,
                clientId: context?.Client.ClientId));
            ModelState.AddModelError(string.Empty, validationResult.Message);
        }

        var vm = await BuildLoginViewModelAsync(model);
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        if (!HttpContext.User.IsAuthenticated()) return RedirectToAction("Login", "Account");

        var userGuid = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == "sub").Value);
        var user = await _context.Users
            .Include(x => x.Sessions)
            .FirstOrDefaultAsync(x => x.Guid == userGuid);
        user?.CloseAllSessions();
        await _context.SaveChangesAsync();

        var vm = await BuildLogoutViewModelAsync(logoutId);

        return await Logout(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(LogoutInputModel model)
    {
        // build a model so the logged out page knows what to display
        var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

        if (User?.Identity.IsAuthenticated == true)
        {
            // delete local authentication cookie
            await HttpContext.SignOutAsync();

            // raise the logout event
            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
        }

        // check if we need to trigger sign-out at an upstream identity provider
        if (vm.TriggerExternalSignout)
        {
            // build a return URL so the upstream provider will redirect back
            // to us after the user has logged out. this allows us to then
            // complete our single sign-out processing.
            string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

            // this triggers a redirect to the external provider for sign-out
            return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
        }

        return View("LoggedOut", vm);
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
        {
            var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

            // this is meant to short circuit the UI and only trigger the one external IdP
            var vm = new LoginViewModel
            {
                EnableLocalLogin = local,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
            };

            if (!local)
            {
                vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
            }

            return vm;
        }

        var schemes = await _schemeProvider.GetAllSchemesAsync();

        var providers = schemes
            .Where(x => x.DisplayName != null)
            .Select(x => new ExternalProvider
            {
                DisplayName = x.DisplayName ?? x.Name,
                AuthenticationScheme = x.Name
            }).ToList();

        var allowLocal = true;
        if (context?.Client.ClientId != null)
        {
            var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
            if (client != null)
            {
                allowLocal = client.EnableLocalLogin;

                if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                {
                    providers = providers.Where(provider =>
                        client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                }
            }
        }

        return new LoginViewModel
        {
            AllowRememberLogin = AccountOptions.AllowRememberLogin,
            EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
            ReturnUrl = returnUrl,
            Username = context?.LoginHint,
            ExternalProviders = providers.ToArray(),
            HasCaptcha = true,
            GeneratedCaptcha = MakeRandom(),
            EnteredCaptcha = ""
        };
    }

    private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
    {
        var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
        vm.Username = model.Username;
        vm.RememberLogin = model.RememberLogin;
        return vm;
    }

    private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
    {
        var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

        if (User?.Identity.IsAuthenticated != true)
        {
            // if the user is not authenticated, then just show logged out page
            vm.ShowLogoutPrompt = false;
            return vm;
        }

        var context = await _interaction.GetLogoutContextAsync(logoutId);
        if (context?.ShowSignoutPrompt == false)
        {
            // it's safe to automatically sign-out
            vm.ShowLogoutPrompt = false;
            return vm;
        }

        // show the logout prompt. this prevents attacks where the user
        // is automatically signed out by another malicious web page.
        return vm;
    }

    private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
    {
        // get context information (client name, post logout redirect URI and iframe for federated signout)
        var logout = await _interaction.GetLogoutContextAsync(logoutId);

        var vm = new LoggedOutViewModel
        {
            AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
            PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
            SignOutIframeUrl = logout?.SignOutIFrameUrl,
            LogoutId = logoutId
        };

        if (User?.Identity.IsAuthenticated == true)
        {
            var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
            {
                var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                if (providerSupportsSignout)
                {
                    if (vm.LogoutId == null)
                    {
                        // if there's no current logout context, we need to create one
                        // this captures necessary info from the current logged in user
                        // before we signout and redirect away to the external IdP for signout
                        vm.LogoutId = await _interaction.CreateLogoutContextAsync();
                    }

                    vm.ExternalAuthenticationScheme = idp;
                }
            }
        }

        return vm;
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePassword command)
    {
        var userGuid = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == "sub").Value);
        var user = await _context.Users
            .Include(x => x.Passwords)
            .FirstOrDefaultAsync(x => x.Guid == userGuid);

        if (user is null) return RedirectToAction("Login");

        var securitySetting = _settingService.Fetch<SecuritySettingViewModel>();
        var passwordLifetimeDays = securitySetting.PasswordLifetimeDays;
        var forbiddenOldPasswordsCount = securitySetting.ForbiddenOldPasswordsCount;
       
        try
        {
            user.GuardAgainsInvalidCurrentPassword(command.CurrentPassword, _passwordHasher);
            user.SetPassword(userGuid, command.Password, passwordLifetimeDays, forbiddenOldPasswordsCount,
                _passwordHasher);

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
        }

        if (ModelState.IsValid)
            return RedirectToAction("Index", "Grants");

        return View();
    }
}