using System.Linq;
using System.Security.Claims;

namespace FRA.Web.Infrastructure.Identity
{
    public static class Extensions
    {
        public static string GetPhotoUrl(this ClaimsPrincipal user)
        {
            return user.Identity.IsAuthenticated
                ? user.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Uri).Value
                : string.Empty;
        }
    }
}
