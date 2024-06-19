using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Phoenix.SSO.SettingModels;
using PhoenixFramework.Application.Setting;
using PhoenixFramework.Domain;
using PhoenixFramework.Identity;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using UserManagement.Domain.UserAgg;
using UserManagement.Persistence;

namespace Phoenix.SSO.IdentitySettings;

public class UserAuthenticationValidator : IResourceOwnerPasswordValidator
{
    private SecuritySettingViewModel Settings { get; set; }

    private readonly IPasswordHasher _passwordHasher;
    private readonly IConnectionStringBuilder _connectionStringBuilder;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAuthenticationValidator(IPasswordHasher passwordHasher, IConnectionStringBuilder connectionStringBuilder,
        IHttpContextAccessor httpContextAccessor, ISettingService settingService)
    {
        _passwordHasher = passwordHasher;
        _connectionStringBuilder = connectionStringBuilder;
        _httpContextAccessor = httpContextAccessor;
        Settings = settingService.Fetch<SecuritySettingViewModel>();
    }

    public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<UserManagementCommandContext>();
        var dbName = context.Request.Raw["dbName"];
        var connectionString = _connectionStringBuilder.Build(dbName);
        dbContextOptionsBuilder.UseSqlServer(connectionString);
        var identityContext = new UserManagementCommandContext(dbContextOptionsBuilder.Options);

        try
        {
            Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
           .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
           .MinimumLevel.Override("System", LogEventLevel.Warning)
           .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
           .Enrich.FromLogContext()
           .WriteTo.Console(
               outputTemplate:
               "222[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
               theme: AnsiConsoleTheme.Code)
           .CreateLogger();


            var user = identityContext
                .Users
                .Include(x => x.Passwords)
                .Include(x => x.Company)
                .Include(x => x.OrganiztionChart)
                .Include(x => x.Sessions
                    .Where(x => x.IsSuccessful)
                    .Where(x => x.IsActive == EntityBase<long>.ActiveStates.Active))
                .Include(u => u.Roles)
                .FirstOrDefault(x => x.Username == context.UserName && !x.IsRemoved);

            if (user is null)
            {
                context.Result =
                    new GrantValidationResult(TokenRequestErrors.InvalidClient,
                        "مشخصات ورود اشتباه است. لطفا دوباره تلاش کنید.");
                return Task.FromResult(context.Result);
            }

            if (user.IsLocked == AuditableAggregateRootBase<int>.LockStates.Lock)
            {
                context.Result = LoginFailed("کاربر قفل شده است، لطفا به راهبر سیستم اطلاع رسانی کنید.");
                return Task.FromResult(context.Result);
            }

            if (LoginAttemptsCountReached(user))
            {
                user.Lock(user.Guid);
                identityContext.SaveChanges();
                context.Result = LoginFailed("کاربر قفل شده است، لطفا به راهبر سیستم اطلاع رسانی کنید.");
                return Task.FromResult(context.Result);
            }

            var clientIpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            if (user.Sessions.Any(x => !x.IsExpired()))
            {
                user.OpenSession(user.Guid, user.NationalCode, user.Fullname, user.Username, user.Company.Title,
                    user.OrganiztionChart.Title, false, clientIpAddress);
                identityContext.SaveChanges();

                context.Result = LoginFailed("کاربر با این مشخصات قبلا وارد سیستم شده است.");
                return Task.FromResult(context.Result);
            }

            var userPassword = user.Passwords
                .First(x => x.IsActive == EntityBase<long>.ActiveStates.Active);

            var (verified, _) = _passwordHasher.Check(userPassword.Password, context.Password);

            if (!verified)
            {
                user.LoginFailed();
                if (LoginAttemptsCountReached(user))
                {
                    user.Lock(user.Guid);
                    identityContext.SaveChanges();
                    context.Result = LoginFailed("کاربر قفل شده است، لطفا به راهبر سیستم اطلاع رسانی کنید.");
                    return Task.FromResult(context.Result);
                }

                user.OpenSession(user.Guid, user.NationalCode, user.Fullname, user.Username, user.Company.Title,
                    user.OrganiztionChart.Title, false, clientIpAddress);
                identityContext.SaveChanges();

                context.Result = LoginFailed("مشخصات ورود اشتباه است. لطفا دوباره تلاش کنید.");
                return Task.FromResult(context.Result);
            }
            Log.Information("before open session ...");
            user.OpenSession(user.Guid, user.NationalCode, user.Fullname, user.Username, user.Company.Title,
                user.OrganiztionChart.Title, true, clientIpAddress, Settings.TokenExpiryTime);

            var firstLogin = identityContext.Users
                .Where(x => x.Guid == user.Guid)
                .SelectMany(x => x.Sessions)
                .Any(x => x.IsSuccessful) == false;

            var shouldChangePassword = firstLogin || userPassword.IsExpired();

            if (shouldChangePassword)
                user.ShouldChangePassword();
            else
                user.PasswordRenewed();

            user.ResetFailedLoginAttempts(user.Guid);
            identityContext.SaveChanges();
            Log.Information("ResetFailedLoginAttempts ...");
            context.Result = new GrantValidationResult(user.Id.ToString(), "Bearer", DateTime.Now,
                new List<Claim>
                {
                    new("id", user.Guid.ToString()),
                    new("role", string.Join(",", user.Roles.Select(x => x.RoleId))),
                    new("dbName", dbName),
                    new("organizationChartGuid", user.OrganiztionChart.Guid.ToString())
                });
            Log.Information("Task.FromResult ...");
            return Task.FromResult(context.Result);
        }
        catch (Exception exception)
        {
            Log.Information(exception.ToString());
            Console.WriteLine(exception);
            throw;
        }
    }

    private bool LoginAttemptsCountReached(User user)
    {
        return user.FailedLoginAttempts >= Settings.LoginAttemptsCountLimit;
    }

    private static GrantValidationResult LoginFailed(string message)
    {
        return new GrantValidationResult(TokenRequestErrors.InvalidClient, message)
        {
            CustomResponse = new Dictionary<string, object> { { "a", "500" } }
        };
    }
}