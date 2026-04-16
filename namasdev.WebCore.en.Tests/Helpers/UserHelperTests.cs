using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using namasdev.WebCore.Helpers;

namespace namasdev.WebCore.Tests.Helpers
{
    public class UserHelperTests
    {
        // ── Helpers ────────────────────────────────────────────────────────────

        private static UserHelper BuildHelper(IEnumerable<Claim>? claims = null, bool authenticated = true)
        {
            var ctx = new DefaultHttpContext();
            if (authenticated)
            {
                var identity = new ClaimsIdentity(claims ?? Enumerable.Empty<Claim>(), "TestAuth");
                ctx.User = new ClaimsPrincipal(identity);
            }
            return new UserHelper(ctx);
        }

        // ── IsLoggedIn ─────────────────────────────────────────────────────────

        [Fact]
        public void IsLoggedIn_AuthenticatedUser_ReturnsTrue()
        {
            var helper = BuildHelper();
            Assert.True(helper.IsLoggedIn);
        }

        [Fact]
        public void IsLoggedIn_AnonymousUser_ReturnsFalse()
        {
            var helper = BuildHelper(authenticated: false);
            Assert.False(helper.IsLoggedIn);
        }

        // ── UserId ─────────────────────────────────────────────────────────────

        [Fact]
        public void UserId_AuthenticatedUser_ReturnsNameIdentifierClaim()
        {
            var helper = BuildHelper([new Claim(ClaimTypes.NameIdentifier, "user-123")]);
            Assert.Equal("user-123", helper.UserId);
        }

        [Fact]
        public void UserId_AnonymousUser_ReturnsNull()
        {
            var helper = BuildHelper(authenticated: false);
            Assert.Null(helper.UserId);
        }

        // ── UserName ───────────────────────────────────────────────────────────

        [Fact]
        public void UserName_AuthenticatedUser_ReturnsIdentityName()
        {
            var helper = BuildHelper([new Claim(ClaimTypes.Name, "john")]);
            Assert.Equal("john", helper.UserName);
        }

        [Fact]
        public void UserName_AnonymousUser_ReturnsNull()
        {
            var helper = BuildHelper(authenticated: false);
            Assert.Null(helper.UserName);
        }

        // ── IsInRole ───────────────────────────────────────────────────────────

        [Fact]
        public void IsInRole_UserInRole_ReturnsTrue()
        {
            var helper = BuildHelper([new Claim(ClaimTypes.Role, "Admin")]);
            Assert.True(helper.IsInRole("Admin"));
        }

        [Fact]
        public void IsInRole_UserNotInRole_ReturnsFalse()
        {
            var helper = BuildHelper([new Claim(ClaimTypes.Role, "Admin")]);
            Assert.False(helper.IsInRole("Manager"));
        }

        [Fact]
        public void IsInRole_CalledTwice_ReturnsSameResult()
        {
            var helper = BuildHelper([new Claim(ClaimTypes.Role, "Admin")]);
            var first = helper.IsInRole("Admin");
            var second = helper.IsInRole("Admin");
            Assert.Equal(first, second);
        }

        // ── IsInAnyRole ────────────────────────────────────────────────────────

        [Fact]
        public void IsInAnyRole_UserInOneOfRoles_ReturnsTrue()
        {
            var helper = BuildHelper([new Claim(ClaimTypes.Role, "Manager")]);
            Assert.True(helper.IsInAnyRole("Admin", "Manager"));
        }

        [Fact]
        public void IsInAnyRole_UserInNoneOfRoles_ReturnsFalse()
        {
            var helper = BuildHelper([new Claim(ClaimTypes.Role, "Viewer")]);
            Assert.False(helper.IsInAnyRole("Admin", "Manager"));
        }

        [Fact]
        public void IsInAnyRole_NullRoles_ReturnsFalse()
        {
            var helper = BuildHelper([new Claim(ClaimTypes.Role, "Admin")]);
            Assert.False(helper.IsInAnyRole(null!));
        }

        [Fact]
        public void IsInAnyRole_EmptyRoles_ReturnsFalse()
        {
            var helper = BuildHelper([new Claim(ClaimTypes.Role, "Admin")]);
            Assert.False(helper.IsInAnyRole());
        }

        // ── GetClaimValue (string) ─────────────────────────────────────────────

        [Fact]
        public void GetClaimValue_ClaimExists_ReturnsValue()
        {
            var helper = BuildHelper([new Claim("department", "Engineering")]);
            Assert.Equal("Engineering", helper.GetClaimValue("department"));
        }

        [Fact]
        public void GetClaimValue_ClaimNotFound_ReturnsNull()
        {
            var helper = BuildHelper();
            Assert.Null(helper.GetClaimValue("department"));
        }

        [Fact]
        public void GetClaimValue_CalledTwice_ReturnsSameResult()
        {
            var helper = BuildHelper([new Claim("department", "Engineering")]);
            var first = helper.GetClaimValue("department");
            var second = helper.GetClaimValue("department");
            Assert.Equal(first, second);
        }

        // ── GetClaimValue<T> ───────────────────────────────────────────────────

        [Fact]
        public void GetClaimValue_Generic_Int_ClaimExists_ReturnsConvertedValue()
        {
            var helper = BuildHelper([new Claim("age", "30")]);
            Assert.Equal(30, helper.GetClaimValue<int>("age"));
        }

        [Fact]
        public void GetClaimValue_Generic_Bool_ClaimExists_ReturnsConvertedValue()
        {
            var helper = BuildHelper([new Claim("active", "True")]);
            Assert.Equal(true, helper.GetClaimValue<bool>("active"));
        }

        [Fact]
        public void GetClaimValue_Generic_ClaimNotFound_ReturnsNull()
        {
            var helper = BuildHelper();
            Assert.Null(helper.GetClaimValue<int>("age"));
        }

        // ── GetClaimValues (string) ────────────────────────────────────────────

        [Fact]
        public void GetClaimValues_MultipleClaimsExist_ReturnsAllValues()
        {
            var helper = BuildHelper(
            [
                new Claim("permission", "Read"),
                new Claim("permission", "Write"),
            ]);
            var values = helper.GetClaimValues("permission").ToList();
            Assert.Equal(2, values.Count);
            Assert.Contains("Read", values);
            Assert.Contains("Write", values);
        }

        [Fact]
        public void GetClaimValues_NoMatchingClaims_ReturnsEmpty()
        {
            var helper = BuildHelper();
            Assert.Empty(helper.GetClaimValues("permission"));
        }

        [Fact]
        public void GetClaimValues_CalledTwice_ReturnsSameResult()
        {
            var helper = BuildHelper(
            [
                new Claim("permission", "Read"),
                new Claim("permission", "Write"),
            ]);
            var first = helper.GetClaimValues("permission").ToList();
            var second = helper.GetClaimValues("permission").ToList();
            Assert.Equal(first, second);
        }

        // ── GetClaimValues<T> ──────────────────────────────────────────────────

        [Fact]
        public void GetClaimValues_Generic_Int_MultipleClaimsExist_ReturnsConvertedValues()
        {
            var helper = BuildHelper(
            [
                new Claim("score", "10"),
                new Claim("score", "20"),
            ]);
            var values = helper.GetClaimValues<int>("score").ToList();
            Assert.Equal(2, values.Count);
            Assert.Contains(10, values);
            Assert.Contains(20, values);
        }

        [Fact]
        public void GetClaimValues_Generic_NoMatchingClaims_ReturnsEmpty()
        {
            var helper = BuildHelper();
            Assert.Empty(helper.GetClaimValues<int>("score"));
        }

        // ── HasClaim (type) ────────────────────────────────────────────────────

        [Fact]
        public void HasClaim_ByType_ClaimExists_ReturnsTrue()
        {
            var helper = BuildHelper([new Claim("department", "Engineering")]);
            Assert.True(helper.HasClaim("department"));
        }

        [Fact]
        public void HasClaim_ByType_ClaimNotFound_ReturnsFalse()
        {
            var helper = BuildHelper();
            Assert.False(helper.HasClaim("department"));
        }

        // ── HasClaim (type, value) ─────────────────────────────────────────────

        [Fact]
        public void HasClaim_ByTypeAndValue_ClaimExistsWithValue_ReturnsTrue()
        {
            var helper = BuildHelper([new Claim("department", "Engineering")]);
            Assert.True(helper.HasClaim("department", "Engineering"));
        }

        [Fact]
        public void HasClaim_ByTypeAndValue_ClaimExistsWithDifferentValue_ReturnsFalse()
        {
            var helper = BuildHelper([new Claim("department", "Engineering")]);
            Assert.False(helper.HasClaim("department", "Marketing"));
        }

        [Fact]
        public void HasClaim_ByTypeAndValue_ClaimNotFound_ReturnsFalse()
        {
            var helper = BuildHelper();
            Assert.False(helper.HasClaim("department", "Engineering"));
        }
    }
}
