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
            new(TransportationApi, "سیستم حمل و نقل"),
            new(UserManagementApi, "مدیریت کاربران")
        };
    }

    public static IEnumerable<Client> Clients(int tokenExpiryTime, string[] allowedOrigins)
    {
        return new List<Client>
        {
            new()
            {
                ClientId = "PhoenixClientCode",
                ClientName = "سامانه کالیبراسیون",
                ClientSecrets = { new Secret("4568_!*&^^%".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "http://localhost:4200/#/challange" },
                FrontChannelLogoutUri = "https://localhost:5001/signout-oidc",
                PostLogoutRedirectUris = { "http://localhost:4200" },

                //RedirectUris = { "http://172.30.12.172:8880/#/challange" },
                //FrontChannelLogoutUri = "http://172.30.12.172:8881/signout-oidc",
                //PostLogoutRedirectUris = { "http://172.30.12.172:8880" },

                IdentityTokenLifetime = tokenExpiryTime,
                AuthorizationCodeLifetime = tokenExpiryTime,
                AccessTokenLifetime = tokenExpiryTime,
                AllowedCorsOrigins = allowedOrigins,
                AllowOfflineAccess = true,
                ClientClaimsPrefix = "",
                AllowedScopes =
                {
                    "openid",
                    "profile",
                    TransportationApi,
                    UserManagementApi
                },


                AlwaysIncludeUserClaimsInIdToken = true,
                AlwaysSendClientClaims = true,
            },
            new()
            {
                ClientId = "PhoenixClientPassword",
                ClientName = "Phoenix Angular Client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowAccessTokensViaBrowser = true,
                IdentityTokenLifetime = tokenExpiryTime,
                AuthorizationCodeLifetime = tokenExpiryTime,
                AccessTokenLifetime = tokenExpiryTime,
                AllowedCorsOrigins = allowedOrigins,
                AllowedScopes =
                {
                    "openid",
                    "profile",
                    TransportationApi,
                    UserManagementApi
                },
                AlwaysIncludeUserClaimsInIdToken = true,
                AlwaysSendClientClaims = true,
                AllowOfflineAccess = true,
                ClientSecrets = new List<Secret> { new("4568_!*&^^%".Sha256()) }
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