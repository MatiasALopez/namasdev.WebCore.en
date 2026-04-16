using System.Security.Claims;

using Microsoft.AspNetCore.Http;

namespace namasdev.WebCore.Helpers
{
    public class UserHelper
    {
        private Dictionary<string, bool>? _roles;
        private Dictionary<string, string?>? _claimValues;
        private Dictionary<string, IEnumerable<string>>? _claimValuesList;

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
                            ? GetClaimValue(ClaimTypes.NameIdentifier)
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
            _roles ??= new Dictionary<string, bool>();
            if (!_roles.TryGetValue(role, out var result))
            {
                result = _context.User.IsInRole(role);
                _roles[role] = result;
            }
            return result;
        }

        public bool IsInAnyRole(params string[] roles)
        {
            if (roles == null || !roles.Any())
            {
                return false;
            }

            return roles.Any(r => IsInRole(r));
        }

        public string? GetClaimValue(string claimType)
        {
            _claimValues ??= new Dictionary<string, string?>();
            if (!_claimValues.TryGetValue(claimType, out var value))
            {
                value = _context.User.FindFirstValue(claimType);
                _claimValues[claimType] = value;
            }
            return value;
        }

        public T? GetClaimValue<T>(string claimType)
            where T : struct
        {
            var value = GetClaimValue(claimType);
            if (value == null)
            {
                return null;
            }
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public IEnumerable<string> GetClaimValues(string claimType)
        {
            _claimValuesList ??= new Dictionary<string, IEnumerable<string>>();
            if (!_claimValuesList.TryGetValue(claimType, out var values))
            {
                values = _context.User.FindAll(claimType).Select(c => c.Value).ToList();
                _claimValuesList[claimType] = values;
            }
            return values;
        }

        public IEnumerable<T> GetClaimValues<T>(string claimType)
            where T : struct
        {
            return GetClaimValues(claimType)
                .Select(v => (T)Convert.ChangeType(v, typeof(T)));
        }

        public bool HasClaim(string claimType)
        {
            return _context.User.HasClaim(c => c.Type == claimType);
        }

        public bool HasClaim(string claimType, string value)
        {
            return _context.User.HasClaim(claimType, value);
        }
    }
}
