using System.Collections.Generic;
using System.Formats.Asn1;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Phoenix.SSO.IdentitySettings;

public class IdentityServiceConfiguration
{
    private const string TransportationApi = "TransportationApi";
    private const string UserManagementApi = "UserManagementApi";

    public static IEnumerable<ApiScope> ApiScopes()
    {
        return new List<ApiScope>
        {
            new(TransportationApi, "سامانه حمل و نقل"),
            new(UserManagementApi, "مدیریت کاربران")
        };
    }

    public static IEnumerable<Client> Clients(int tokenExpiryTime, string[] allowedOrigins)
    {
        return new List<Client>
        {
            new()
            {
                ClientId = "TransportationClient",
                ClientName = "سامانه حمل و نقل",
                ClientSecrets = { new Secret("FJOIEf9wnfnv#*(*2489".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "http://localhost:4200/#/challange" },
                FrontChannelLogoutUri = "https://localhost:5001/signout-oidc",
                PostLogoutRedirectUris = { "http://localhost:4200" },

                IdentityTokenLifetime = tokenExpiryTime,
                AuthorizationCodeLifetime = tokenExpiryTime,
                AccessTokenLifetime = tokenExpiryTime,
                AllowedCorsOrigins = allowedOrigins,
                AllowOfflineAccess = true,
                ClientClaimsPrefix = "",
                AlwaysIncludeUserClaimsInIdToken = true,
                AlwaysSendClientClaims = true,
                AllowedScopes =
                {
                    "openid",
                    "profile",
                    TransportationApi,
                    UserManagementApi
                },
            },
            new()
            {
                ClientId = "UserManagementClient",
                ClientName = "سامانه مدیریت کاربران",
                ClientSecrets = { new Secret("fjsdFDSJ98(&^&%^(CV".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "http://localhost:5200/#/challange" },
                FrontChannelLogoutUri = "https://localhost:5001/signout-oidc",
                PostLogoutRedirectUris = { "http://localhost:5200" },
                
                IdentityTokenLifetime = tokenExpiryTime,
                AuthorizationCodeLifetime = tokenExpiryTime,
                AccessTokenLifetime = tokenExpiryTime,
                AllowedCorsOrigins = allowedOrigins,
                AllowOfflineAccess = true,
                ClientClaimsPrefix = "",
                AlwaysIncludeUserClaimsInIdToken = true,
                AlwaysSendClientClaims = true,
                AllowedScopes =
                {
                    "openid",
                    "profile",
                    UserManagementApi
                },
            }
        };
    }

    public static IEnumerable<IdentityResource> IdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId
            {
                UserClaims = new[]
                {
                    "id",
                    ClaimTypes.Role,
                    ClaimTypes.Email,
                    "Guid"
                }
            },
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };
    }
}