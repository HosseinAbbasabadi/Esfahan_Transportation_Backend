using System;
using System.Linq;
using PhoenixFramework.Domain;
using PhoenixFramework.Identity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PhoenixFramework.Core.Exceptions;
using UserManagement.Domain.CompanyAgg;
using UserManagement.Domain.OrganizationChartAgg;
using UserManagement.Domain.RoleAgg;

namespace UserManagement.Domain.UserAgg;

public class User : AuditableAggregateRootBase<int>
{
    private readonly IList<UserClaim> _claims;

    public string Username { get; private set; }
    public string NationalCode { get; private set; }
    public string Fullname { get; private set; }
    public string Mobile { get; private set; }
    public long ClassificationLevelId { get; private set; }
    public string EmployeeCode { get; private set; }
    public long CompanyId { get; private set; }
    public long OrganizationChartId { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public bool PasswordExpired { get; private set; }
    public List<UserSession> Sessions { get; private set; }
    public List<UserPassword> Passwords { get; private set; }
    public Company Company { get; private set; }
    public OrganizationChart OrganiztionChart { get; private set; }
    public List<UserRole> Roles { get; private set; }
    public List<UserSystem> Systems { get; private set; }
    public IReadOnlyCollection<UserClaim> Claims => new ReadOnlyCollection<UserClaim>(_claims);

    protected User()
    {
    }

    public User(Guid actor, IEnumerable<int> roleIds, IEnumerable<int> systemIds, string username, string nationalCode,
        string mobile, string fullname, long classificationLevelId, string employeeCode, long companyId,
        long organizationChartId) : base(actor)
    {
        NationalCode = nationalCode;
        Username = username;
        Fullname = fullname;
        Mobile = mobile;
        EmployeeCode = employeeCode;
        ClassificationLevelId = classificationLevelId;
        CompanyId = companyId;
        OrganizationChartId = organizationChartId;
        FailedLoginAttempts = 0;
        Roles = roleIds.Select(x => new UserRole(Id, x)).ToList();
        Systems = systemIds.Select(x => new UserSystem(Id, x)).ToList();

        ShouldChangePassword();
    }

    public void Edit(IEnumerable<int> roleIds, List<int> systemIds, string fullname, string username,
        string nationalCode, string mobile, long classificationLevelId, string employeeCode, long companyId,
        long organizationChartId)
    {
        NationalCode = nationalCode;
        EmployeeCode = employeeCode;
        ClassificationLevelId = classificationLevelId;
        CompanyId = companyId;
        OrganizationChartId = organizationChartId;
        Roles = roleIds.Select(x => new UserRole(Id, x)).ToList();
        Systems = systemIds.Select(x => new UserSystem(Id, x)).ToList();

        if (!string.IsNullOrEmpty(fullname)) Fullname = fullname;
        if (!string.IsNullOrEmpty(username)) Username = username;
        if (!string.IsNullOrEmpty(mobile)) Mobile = mobile;
    }

    public void OpenSession(Guid actor, string nationalCode, string userFullname, string username, string companytitle,
        string organizationChartTitle, bool isSuccessful, string clientIpAddress, int? tokenExpireTime = null)
    {
        var session = new UserSession(actor, nationalCode, userFullname, username, companytitle,
            organizationChartTitle, isSuccessful, clientIpAddress, tokenExpireTime);
        Sessions ??= new List<UserSession>();
        Sessions.Add(session);
    }

    public void LoginFailed()
    {
        FailedLoginAttempts += 1;
    }

    public void GuardAgainsInvalidCurrentPassword(string password, IPasswordHasher passwordHasher)
    {
        var currentPassword = Passwords.First(x => x.IsActive == EntityBase<long>.ActiveStates.Active).Password;

        var hash = passwordHasher.Hash(password);
        var (verified, _) = passwordHasher.Check(currentPassword, password);

        if (!verified)
            throw new BusinessException("0", "کلمه رمز فعلی صحیح نمی باشد.");
    }

    public void SetPassword(Guid actor, string password, int passwordLifetimeDays, int forbiddenOldPasswordsCount,
        IPasswordHasher passwordHasher)
    {
        if (Passwords is not null)
        {
            foreach (var item in Passwords.OrderByDescending(x => x.Created).Take(forbiddenOldPasswordsCount))
            {
                var (verified, _) = passwordHasher.Check(item.Password, password);
                if (verified)
                    throw new BusinessException("0",
                        "از این کلمه رمز قبلا استفاده شده است. امکان درج کلمه رمز تکراری وجود ندارد.");

                item.Deactivate();
            }
        }
        else
            Passwords = new List<UserPassword>();

        var hash = passwordHasher.Hash(password);
        var expireDate = DateTime.Now.AddDays(passwordLifetimeDays).Date;
        var newPassword = new UserPassword(actor, Id, hash, expireDate);
        Passwords.Add(newPassword);

        if (actor != Guid)
            ShouldChangePassword();
        else
            PasswordRenewed();
    }

    public void ShouldChangePassword()
    {
        PasswordExpired = true;
    }

    public void PasswordRenewed()
    {
        PasswordExpired = false;
    }

    public void CloseAllSessions()
    {
        foreach (var session in Sessions) session.Deactivate();
    }

    public void ResetFailedLoginAttempts(Guid actor)
    {
        Unlock(actor);
        FailedLoginAttempts = 0;
    }
}