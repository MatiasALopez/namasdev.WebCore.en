using System.Security.Claims;

using Microsoft.AspNetCore.Http;

namespace namasdev.WebCore.Helpers
{
    public class UserHelper
    {
        private readonly HttpContext _context;

        public UserHelper(HttpContext context)
        {
            _context = context;
        }

        public bool IsLoggedIn
        {
            get { return _context.User.Identity?.IsAuthenticated == true; }
        }

        private string? _userId;
        public string? UserId
        {
            get
            {
                return
                    _userId
                    ?? (_userId = IsLoggedIn
                            ? _context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                            : null);
            }
        }

        private string? _userName;
        public string? UserName
        {
            get
            {
                return
                    _userName
                    ?? (_userName = IsLoggedIn
                            ? _context.User.Identity?.Name
                            : null);
            }
        }

        public bool IsInRole(string role)
        {
            return _context.User.IsInRole(role);
        }

        public bool IsInAnyRole(params string[] roles)
        {
            if (roles == null || !roles.Any())
            {
                return false;
            }

            return roles.Any(r => _context.User.IsInRole(r));
        }
    }
}
