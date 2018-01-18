using System.Linq;
using System.Security.Claims;

namespace CMSCore.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetSpecificClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var claim = claimsPrincipal.Claims.FirstOrDefault(m => m.Type == claimType);
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}