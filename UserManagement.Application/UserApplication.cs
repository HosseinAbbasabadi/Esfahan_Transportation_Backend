using System;
using PhoenixFramework.Identity;
using System.Collections.Generic;
using UserManagement.Domain.UserAgg;
using PhoenixFramework.Core.Exceptions;
using Microsoft.Extensions.Configuration;
using PhoenixFramework.Application.Query;
using PhoenixFramework.Application.Setting;
using UserManagement.Domain.ClassificationLevelAgg;
using UserManagement.Application.Contracts.Contracts;
using UserManagement.Application.Contracts.ViewModels;
using UserManagement.Application.Contracts.SearchModels;
using UserManagement.Application.Contracts.Commands.User;
using UserManagement.Domain.CompanyAgg;
using UserManagement.Domain.OrganizationChartAgg;
using UserManagement.Domain.RoleAgg;
using UserManagement.Domain.SystemAgg;

namespace UserManagement.Application;

public class UserApplication : IUserApplication
{
    private readonly IClaimHelper _claimHelper;
    private readonly IQueryBus _queryBus;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRoleRepository _roleRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IOrganizationChartRepository _organizationChartRepository;
    private readonly IClassificationLevelRepository _classificationLevelRepository;
    private readonly ISettingService _settingService;
    private readonly ISystemRepository _systemRepository;

    public UserApplication(IUserRepository userRepository, IPasswordHasher passwordHasher,
        IQueryBus queryBus, IClaimHelper claimHelper, ICompanyRepository companyRepository,
        IOrganizationChartRepository organizationChartRepository, IRoleRepository roleRepository,
        IClassificationLevelRepository classificationLevelRepository, ISettingService settingService,
        ISystemRepository systemRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _queryBus = queryBus;
        _claimHelper = claimHelper;
        _companyRepository = companyRepository;
        _organizationChartRepository = organizationChartRepository;
        _roleRepository = roleRepository;
        _classificationLevelRepository = classificationLevelRepository;
        _settingService = settingService;
        _systemRepository = systemRepository;
    }

    public UserViewModel Login(Login command)
    {
        var user = _userRepository.GetByUsername(command.Username);

        if (user == null)
            throw new BusinessException("0", "نام کاربری یا کلمه عبور اشتباه است.");

        return new UserViewModel
        {
            Id = user.Id,
            Fullname = user.Fullname,
            Username = user.Username,
        };
    }

    public void ChangePassword(ChangePassword command)
    {
        var actor = _claimHelper.GetCurrentUserGuid();
        var user = _userRepository.Load(actor, "Passwords");

        var securitySetting = _settingService.Fetch<SecuritySettingViewModel>();
        var passwordLifetimeDays = securitySetting.PasswordLifetimeDays;
        var forbiddenOldPasswordsCount = securitySetting.ForbiddenOldPasswordsCount;

        user.GuardAgainsInvalidCurrentPassword(command.CurrentPassword, _passwordHasher);
        user.SetPassword(actor, command.Password, passwordLifetimeDays, forbiddenOldPasswordsCount, _passwordHasher);

        _userRepository.Update(user);
        _userRepository.SaveChanges();
    }

    public void Create(CreateUser command)
    {
        var creator = _claimHelper.GetCurrentUserGuid();

        var companyId = _companyRepository.GetIdBy(command.CompanyGuid);
        var organizationChartId = _organizationChartRepository.GetIdBy(command.OrganizationChartGuid);
        var roleIds = _roleRepository.GetBatchIdBy(command.RoleGuids);
        var systemIds = _systemRepository.GetBatchIdBy(command.SystemGuids);
        var classificationLevelId = _classificationLevelRepository.GetIdBy(command.ClassificationLevelGuid);

        if (_userRepository.Exists(x => x.Username == command.Username))
            throw new BusinessException("0", "کاربر با این نام کاربری قبلا ثبت شده است.");

        var user = new User(creator, roleIds, systemIds, command.Username, command.NationalCode, command.Mobile,
            command.Fullname, classificationLevelId, command.EmployeeCode, companyId, organizationChartId);

        var securitySetting = _settingService.Fetch<SecuritySettingViewModel>();
        var passwordLifetimeDays = securitySetting.PasswordLifetimeDays;
        var forbiddenOldPasswordsCount = securitySetting.ForbiddenOldPasswordsCount;

        user.SetPassword(creator, command.Password, passwordLifetimeDays, forbiddenOldPasswordsCount, _passwordHasher);

        _userRepository.Create(user);
        _userRepository.SaveChanges();
    }

    public void Edit(EditUser command)
    {
        var actor = _claimHelper.GetCurrentUserGuid();
        var user = _userRepository.Load(command.Guid, "Passwords,Roles,Systems");
        var companyId = _companyRepository.GetIdBy(command.CompanyGuid);
        var organizationChartId = _organizationChartRepository.GetIdBy(command.OrganizationChartGuid);
        var roleIds = _roleRepository.GetBatchIdBy(command.RoleGuids);
        var systemIds = _systemRepository.GetBatchIdBy(command.SystemGuids);
        var classificationLevelId = _classificationLevelRepository.GetIdBy(command.ClassificationLevelGuid);

        if (_userRepository.Exists(x => x.Username == command.Username && x.Guid != command.Guid))
            throw new BusinessException("0", "کاربر با این نام کاربری قبلا ثبت شده است.");

        user.Edit(roleIds, systemIds, command.Fullname, command.Username, command.NationalCode, command.Mobile,
            classificationLevelId, command.EmployeeCode, companyId, organizationChartId);

        if (!string.IsNullOrWhiteSpace(command.Password))
        {
            var securitySetting = _settingService.Fetch<SecuritySettingViewModel>();
            var passwordLifetimeDays = securitySetting.PasswordLifetimeDays;
            var forbiddenOldPasswordsCount = securitySetting.ForbiddenOldPasswordsCount;
            user.SetPassword(actor, command.Password, passwordLifetimeDays, forbiddenOldPasswordsCount,
                _passwordHasher);
        }

        _userRepository.Update(user);
        _userRepository.SaveChanges();
    }

    public EditUser GetBy(Guid guid)
    {
        return _queryBus.Dispatch<EditUser, EditUserSearchModel>(new EditUserSearchModel(guid));
    }

    public List<UserViewModel> GetList()
    {
        return _queryBus.Dispatch<List<UserViewModel>>();
    }

    public void Delete(Guid id)
    {
        var user = _userRepository.Load(id);

        _userRepository.Delete(user);
        _userRepository.SaveChanges();
    }

    public void Lock(Guid guid)
    {
        var currentUserGuid = _claimHelper.GetCurrentUserGuid();
        var user = _userRepository.Load(guid);
        user.Lock(user.Guid);

        _userRepository.SaveChanges();
    }

    public void Unlock(Guid guid)
    {
        var currentUserGuid = _claimHelper.GetCurrentUserGuid();
        var user = _userRepository.Load(guid);
        user.ResetFailedLoginAttempts(currentUserGuid);

        _userRepository.SaveChanges();
    }

    public void OpenSession(OpenSession command)
    {
        var securitySetting = _settingService.Fetch<SecuritySettingViewModel>();
        var tokenExpiryTime = securitySetting.TokenExpiryTime;
        var currentUserGuid = _claimHelper.GetCurrentUserGuid();
        var user = _userRepository.Load(command.Guid, "Sessions,Company,OrganiztionChart");
        user.OpenSession(currentUserGuid, user.NationalCode, user.Fullname, user.Username, user.Company.Title,
            user.OrganiztionChart.Title, command.IsSuccessful, command.ClientIpAddress, tokenExpiryTime);

        _userRepository.Update(user);
        _userRepository.SaveChanges();
    }

    public void CloseSession(Guid guid)
    {
        var user = _userRepository.Load(guid, "Sessions");
        user.CloseAllSessions();

        _userRepository.SaveChanges();
    }
}