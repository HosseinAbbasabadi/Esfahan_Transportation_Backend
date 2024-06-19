using System;
using PhoenixFramework.Domain;

namespace UserManagement.Domain.UserAgg;

public class UserSession : EntityBase<long>
{
    public Guid Guid { get; private set; }
    public Guid UserGuid { get; private set; }
    public string NationalCode { get; private set; }
    public string UserFullname { get; private set; }
    public string Username { get; private set; }
    public string CompanyTitle { get; private set; }
    public string OrganizationChartTitle { get; private set; }
    public bool IsSuccessful { get; private set; }
    public DateTime? ExpireDate { get; private set; }
    public string ClientIpAddress { get; private set; }
    public User User { get; private set; }

    protected UserSession() { }

    public UserSession(Guid creator, string nationalCode, string userFullname, string username, string companytitle,
        string organizationChartTitle, bool isSuccessful, string clientIpAddress, int? tokenExpireTime = null) : base(creator)
    {
        Guid = Guid.NewGuid();
        NationalCode = nationalCode;
        UserFullname = userFullname;
        Username = username;
        CompanyTitle = companytitle;
        OrganizationChartTitle = organizationChartTitle;
        IsSuccessful = isSuccessful;
        ClientIpAddress = clientIpAddress;

        if (tokenExpireTime is not null)
            ExpireDate = DateTime.Now.AddHours(tokenExpireTime.Value);
    }

    public bool IsExpired()
    {
        return IsSuccessful &&
               IsActive == ActiveStates.Active &&
               ExpireDate < DateTime.Now;
    }
}