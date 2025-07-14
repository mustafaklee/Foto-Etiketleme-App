using System.Security.Claims;

namespace WebAPI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            return int.TryParse(idClaim?.Value, out var id) ? id : (int?)null;
        }
    }
}
