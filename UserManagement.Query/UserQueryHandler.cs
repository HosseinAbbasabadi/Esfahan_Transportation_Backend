using System;
using System.Linq;
using System.Threading.Tasks;
using PhoenixFramework.Domain;
using PhoenixFramework.Dapper;
using PhoenixFramework.Identity;
using System.Collections.Generic;
using UserManagement.Persistence;
using PhoenixFramework.Application;
using Microsoft.EntityFrameworkCore;
using PhoenixFramework.Application.Query;
using PhoenixFramework.Application.Setting;
using UserManagement.Query.Contracts.User;
using UserManagement.Application.Contracts.ViewModels;
using UserManagement.Application.Contracts.SearchModels;
using UserManagement.Application.Contracts.Commands.User;
using PhoenixFramework.Core.Exceptions;
using UserManagement.Query.Contracts.Setting;

namespace UserManagement.Query;

public class UserQueryHandler :
    IQueryHandler<List<UserViewModel>>,
    IQueryHandler<EditUser, EditUserSearchModel>,
    IQueryHandler<List<UserComboModel>>,
    IQueryHandler<List<UserComboModel>, Guid>,
    IQueryHandlerAsync<UserInformationViewModel>,
    IQueryHandlerAsync<List<UserSessionViewModel>>,
    IQueryHandlerAsync<UserActiveSessionViewModel>,
    IQueryHandlerAsync<List<UserSessionViewModel>, UserSessionSearchModel>
{
    private const string UserSpName = "spGetUserFor";
    private readonly BaseDapperRepository _repository;
    private readonly UserManagementQueryContext _context;
    private readonly IClaimHelper _claimHelper;
    private List<long> _orgIds = new();
    private readonly ISettingService _settingService;
    public List<long> CompanyLongs = new List<long>();

    public UserQueryHandler(BaseDapperRepository repository, IClaimHelper claimHelper,
        UserManagementQueryContext context, ISettingService settingService)
    {
        _repository = repository;
        _claimHelper = claimHelper;
        _context = context;
        _settingService = settingService;
    }

    List<UserViewModel> IQueryHandler<List<UserViewModel>>.Handle()
    {
        return _repository.SelectFromSp<UserViewModel>(UserSpName, new { Type = QueryOutputs.List });
    }

    public EditUser Handle(EditUserSearchModel searchModel)
    {
        var user = _repository.SelectFromSpFirstOrDefault<EditUser>(UserSpName,
            new { Type = QueryOutputs.Edit, searchModel.Guid });

        var roleIds = _context.Users
            .Include(x => x.Roles)
            .Where(x => x.Guid == user.Guid)
            .SelectMany(x => x.Roles.Select(x => x.RoleId))
            .ToList();

        user.RoleGuids = _context.Roles
            .Where(x => roleIds.Contains(x.Id))
            .Select(x => x.Guid)
            .ToList();

        var systemIds = _context.Users
            .Include(x => x.Systems)
            .Where(x => x.Guid == user.Guid)
            .SelectMany(x => x.Systems.Select(x => x.SystemId))
            .ToList();

        user.SystemGuids = _context.Systems
            .Where(x => x.IsActive == 1)
            .Where(x => systemIds.Contains(x.Id))
            .Select(x => x.Guid)
            .ToList();

        return user;
    }

    List<UserComboModel> IQueryHandler<List<UserComboModel>>.Handle()
    {
        return _repository.SelectFromSp<UserComboModel>(UserSpName, new { Type = QueryOutputs.Combo });
    }

    public async Task<UserInformationViewModel> Handle()
    {
        var currentUserGuid = _claimHelper.GetCurrentUserGuid();

        var userInfo = await (from user in _context.Users
            join cl in _context.ClassificationLevel on
                user.ClassificationLevelId equals cl.Id
            where user.Guid == currentUserGuid
            select new
            {
                user.Fullname,
                ClassificationLevelTitle = cl.Title,
                ClassificationLevelGuid = cl.Guid,
                user.CompanyId,
                user.OrganizationChartId,
                user.PasswordExpired,
                user.NationalCode
            }).FirstOrDefaultAsync();

        var organizationChart = await _context.OrganizationCharts
            .Select(x => new { x.Id, x.Guid, x.Title })
            .FirstOrDefaultAsync(x => x.Id == userInfo.OrganizationChartId);

        var company = await _context.Companies
            .Select(x => new { x.Id, x.Guid, x.Title })
            .FirstOrDefaultAsync(x => x.Id == userInfo.CompanyId);

        return new UserInformationViewModel
        {
            CompanyGuid = company.Guid,
            Fullname = userInfo.Fullname,
            CompanyTitle = company.Title,
            NeedChangePassword = userInfo.PasswordExpired,
            OrganizationChartGuid = organizationChart.Guid,
            OrganizationChartTitle = organizationChart.Title,
            ClassificationLevel = userInfo.ClassificationLevelTitle,
            ClassificationLevelGuid = userInfo.ClassificationLevelGuid,
        };
    }

    async Task<List<UserSessionViewModel>> IQueryHandlerAsync<List<UserSessionViewModel>>.Handle()
    {
        var currentUserGuid = _claimHelper.GetCurrentUserGuid();
        var numberOfUserSessions = _settingService.Fetch<UserManagementSettingViewModel>().NumberOfUserSessionsToShow;

        return await _context.Users
            .Where(x => x.Guid == currentUserGuid)
            .Where(x => !x.PasswordExpired)
            .SelectMany(x => x.Sessions)
            .Select(x => new UserSessionViewModel
            {
                Guid = x.Guid,
                IsSuccessful = x.IsSuccessful,
                ClientIpAddress = x.ClientIpAddress,
                Created = x.Created.ToFarsiFull(),
                CreatedEng = x.Created
            }).OrderByDescending(x => x.CreatedEng)
            .Take(numberOfUserSessions)
            .ToListAsync();
    }

    async Task<UserActiveSessionViewModel> IQueryHandlerAsync<UserActiveSessionViewModel>.Handle()
    {
        var currentUserGuid = _claimHelper.GetCurrentUserGuid();
        var session = await _context.Users
            .Where(x => x.Guid == currentUserGuid)
            .SelectMany(x => x.Sessions)
            .Where(x => x.IsSuccessful)
            .Where(x => x.IsActive == EntityBase<long>.ActiveStates.Active)
            .OrderByDescending(x => x.Created)
            .FirstOrDefaultAsync();

        if (session is null) return new UserActiveSessionViewModel();

        return new UserActiveSessionViewModel
        {
            IsActive = !session.IsExpired()
        };
    }

    public async Task<List<UserSessionViewModel>> Handle(UserSessionSearchModel searchModel)
    {
        searchModel.StartDate = searchModel.StartDatePer.ToGeorgianDateTime();
        searchModel.EndDate = searchModel.EndDatePer.ToGeorgianDateTime();
        if (searchModel.EndDate < searchModel.StartDate)
            throw new BusinessException("", "تاریخ پایان باید بزرگتر از تاریخ شروع باشد");
        var usersessions = (from session in _context.UserSessions
            where session.Created.Date >= searchModel.StartDate.Value.Date
                  && session.Created.Date <= searchModel.EndDate.Value.Date
            select (new UserSessionViewModel
            {
                Guid = session.Guid,
                UserGuid = session.UserGuid,
                IsSuccessful = session.IsSuccessful,
                IsSuccessfulTitle = session.IsSuccessful ? "ورود موفق" : "ورود ناموفق",
                ClientIpAddress = session.ClientIpAddress,
                Created = session.Created.ToFarsiFull(),
                CreatedEng = session.Created,
                Fullname = session.UserFullname,
                Username = session.Username,
                CompanyTitle = session.CompanyTitle,
                OrganizationChartTitle = session.OrganizationChartTitle,
                NationalCode = session.NationalCode,
            }));

        if (searchModel.UserGuid != null)
            usersessions = usersessions
                .Where(x => x.UserGuid == searchModel.UserGuid);

        if (searchModel.ClientIpAddress != "")
            usersessions = usersessions
                .Where(x => x.ClientIpAddress == searchModel.ClientIpAddress);

        return usersessions.OrderByDescending(x => x.CreatedEng).ToList();
    }

    public List<UserComboModel> Handle(Guid organizationChartGuid)
    {
        var comid = _context.OrganizationCharts
            .Where(x => x.Guid == organizationChartGuid).FirstOrDefault()?.CompanyId;
        var company = _context
            .Companies
            .FirstOrDefault(x => x.Id == comid);
        if (company != null)
        {
            GetCompanyIds(company.Id);
            _orgIds = _context.OrganizationCharts.Where(x => CompanyLongs.Contains(x.CompanyId)).Select(x => x.Id)
                .ToList();
        }
        else
        {
            var organizationChartId = _context.Companies.FirstOrDefault(x => x.Guid == organizationChartGuid)?.Id;
            GetOrgIds(organizationChartId ?? 0);
        }

        var users = _context.Users
            .Where(x => _orgIds.Contains(x.OrganizationChartId))
            .Select(x => new UserComboModel
            {
                Guid = x.Guid,
                Title = x.Fullname,
            }).Take(10).ToList();

        return users;
    }

    private void GetOrgIds(long orgid)
    {
        _orgIds.Add(orgid);
        var org = _context.OrganizationCharts.Where(x => x.ParentId == orgid).Select(x => x.Id).ToList().Distinct();
        foreach (var item in org)
            GetOrgIds(item);
    }

    private void GetCompanyIds(long? comId)
    {
        if (comId != null)
        {
            CompanyLongs.Add(comId ?? 0);
            var comid = _context.Companies.Where(x => x.Id == comId).FirstOrDefault()?.Id;
            var companys = _context.Companies.Where(x => x.ParentId == comid).Select(x => x.Id).ToList().Distinct();
            foreach (var item in companys)
                GetCompanyIds(item);
        }
    }
}