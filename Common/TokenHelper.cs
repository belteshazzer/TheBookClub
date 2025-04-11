using System.Security.Claims;

namespace TheBookClub.Common
{
    public static class TokenHelper
    {
        /// <summary>
        /// Retrieves the User ID from the access token.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>The User ID as a string, or null if not found.</returns>
        public static Guid GetUserId(HttpContext httpContext)
        {
            var userIdString = httpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (Guid.TryParse(userIdString, out var userId))
            {
                return userId;
            }

            throw new InvalidOperationException("Invalid or missing User ID in the access token.");
        }

        /// <summary>
        /// Retrieves the Email from the access token.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>The Email as a string, or null if not found.</returns>
        public static string GetEmail(HttpContext httpContext)
        {
            return httpContext?.User?.FindFirstValue(ClaimTypes.Email);
        }

        /// <summary>
        /// Retrieves the Roles from the access token.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>A list of roles, or an empty list if no roles are found.</returns>
        public static IEnumerable<string> GetRoles(HttpContext httpContext)
        {
            return httpContext?.User?.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value) ?? Enumerable.Empty<string>();
        }

        /// <summary>
        /// Retrieves a custom claim value from the access token.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <param name="claimType">The type of the claim to retrieve.</param>
        /// <returns>The claim value as a string, or null if not found.</returns>
        public static string GetCustomClaim(HttpContext httpContext, string claimType)
        {
            return httpContext?.User?.FindFirstValue(claimType);
        }
    }
}