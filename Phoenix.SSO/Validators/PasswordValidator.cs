using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Phoenix.SSO.SettingModels;
using PhoenixFramework.Application.Setting;
using PhoenixFramework.Domain;
using PhoenixFramework.Identity;
using UserManagement.Domain.UserAgg;
using UserManagement.Persistence;

namespace Phoenix.SSO.Validators;

public class PasswordValidator : IPasswordValidator
{
    private SecuritySettingViewModel Settings { get; set; }

    private readonly UserManagementCommandContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PasswordValidator(UserManagementCommandContext context, IPasswordHasher passwordHasher,
        IHttpContextAccessor httpContextAccessor, ISettingService settingService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _httpContextAccessor = httpContextAccessor;
        Settings = settingService.Fetch<SecuritySettingViewModel>();
    }

    public PasswordValidationResult Validate(string username, string password)
    {
        var result = new PasswordValidationResult();

        var user = _context
            .Users
            .Include(x => x.Passwords)
            .Include(x => x.Company)
            .Include(x => x.OrganiztionChart)
            .Include(x => x.Sessions
                .Where(x => x.IsSuccessful)
                .Where(x => x.IsActive == EntityBase<long>.ActiveStates.Active))
            .Include(u => u.Roles)
            .FirstOrDefault(x => x.Username == username && !x.IsRemoved);

        if (user is null)
            return result.Failed("مشخصات ورود اشتباه است. لطفا دوباره تلاش کنید.");

        if (user.IsLocked == AuditableAggregateRootBase<int>.LockStates.Lock)
            return result.Failed("کاربر قفل شده است، لطفا به راهبر سیستم اطلاع رسانی کنید.");

        if (LoginAttemptsCountReached(user))
        {
            user.Lock(user.Guid);
            _context.SaveChanges();

            return result.Failed("کاربر قفل شده است، لطفا به راهبر سیستم اطلاع رسانی کنید.");
        }

        var clientIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        if (user.Sessions.Any(x => !x.IsExpired()))
        {
            user.OpenSession(user.Guid, user.NationalCode, user.Fullname, user.Username, user.Company.Title,
                user.OrganiztionChart.Title, false, clientIpAddress);
            _context.SaveChanges();

            return result.Failed("کاربر با این مشخصات قبلا وارد سیستم شده است.");
        }

        var userPassword = user.Passwords
            .First(x => x.IsActive == EntityBase<long>.ActiveStates.Active);

        var (verified, _) = _passwordHasher.Check(userPassword.Password, password);

        if (!verified)
        {
            user.LoginFailed();
            if (LoginAttemptsCountReached(user))
            {
                user.Lock(user.Guid);
                _context.SaveChanges();

                return result.Failed("کاربر قفل شده است، لطفا به راهبر سیستم اطلاع رسانی کنید.");
            }

            user.OpenSession(user.Guid, user.NationalCode, user.Fullname, user.Username, user.Company.Title,
                user.OrganiztionChart.Title, false, clientIpAddress);
            _context.SaveChanges();

            return result.Failed("مشخصات ورود اشتباه است. لطفا دوباره تلاش کنید.");
        }

        var tokenExpiryTime = Settings.TokenExpiryTime;
        user.OpenSession(user.Guid, user.NationalCode, user.Fullname, user.Username, user.Company.Title,
            user.OrganiztionChart.Title, true, clientIpAddress, tokenExpiryTime);

        var firstLogin = _context.Users
            .Where(x => x.Guid == user.Guid)
            .SelectMany(x => x.Sessions)
            .Any(x => x.IsSuccessful) == false;

        var shouldChangePassword = firstLogin || userPassword.IsExpired();

        if (shouldChangePassword)
            user.ShouldChangePassword();
        else
            user.PasswordRenewed();

        user.ResetFailedLoginAttempts(user.Guid);
        _context.SaveChanges();

        return result.Success();
    }

    private bool LoginAttemptsCountReached(User user)
    {
        return user.FailedLoginAttempts >= Settings.LoginAttemptsCountLimit;
    }
}