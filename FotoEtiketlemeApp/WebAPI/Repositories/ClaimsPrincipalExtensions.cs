
using System.Security.Claims;

namespace WebAPI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            return Guid.TryParse(idClaim?.Value, out var id) ? id : (Guid?)null;
        }
    }
}
