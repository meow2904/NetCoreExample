
using System.Security.Claims;
using System.Security.Principal;
using NetCoreEx.Utilities.Constants;

namespace NetCoreEx.WebApp.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetEmail(this IIdentity? identity)
        {
            ClaimsIdentity? claimsIdentity = identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(ClaimTypes.Email);
            return claim?.Value ?? "";
        }

        public static string GetAvatar(this IIdentity? identity)
        {
            ClaimsIdentity? claimsIdentity = identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(CustomClaimTypes.Avatar);

            return claim?.Value ?? string.Empty;
        }
        public static string GetUserId(this IIdentity? identity)
        {
            ClaimsIdentity? claimsIdentity = identity as ClaimsIdentity;
            Claim? claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            return claim?.Value ?? string.Empty;
        }
    }
}