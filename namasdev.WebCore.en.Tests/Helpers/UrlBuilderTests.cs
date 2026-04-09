using Microsoft.AspNetCore.Http;

using namasdev.WebCore.Helpers;

namespace namasdev.WebCore.Tests.Helpers
{
    public class UrlBuilderTests
    {
        // ── Helpers ────────────────────────────────────────────────────────────

        private static HttpRequest BuildRequest(string path, string? queryString = null)
        {
            var ctx = new DefaultHttpContext();
            ctx.Request.Scheme = "https";
            ctx.Request.Host = new HostString("example.com");
            ctx.Request.Path = path;
            ctx.Request.QueryString = queryString != null
                ? new QueryString(queryString)
                : QueryString.Empty;
            return ctx.Request;
        }

        // ── BuildUrlWithPage (HttpRequest) ─────────────────────────────────────

        [Fact]
        public void BuildUrlWithPage_HttpRequest_Page1_RemovesPageParam()
        {
            var request = BuildRequest("/list", "?page=3");
            var result = UrlBuilder.BuildUrlWithPage(request, 1);
            Assert.Equal("/list", result);
        }

        [Fact]
        public void BuildUrlWithPage_HttpRequest_PageGreaterThan1_SetsPageParam()
        {
            var request = BuildRequest("/list");
            var result = UrlBuilder.BuildUrlWithPage(request, 2);
            Assert.Equal("/list?page=2", result);
        }

        [Fact]
        public void BuildUrlWithPage_HttpRequest_PreservesOtherParams()
        {
            var request = BuildRequest("/list", "?order=name&page=1");
            var result = UrlBuilder.BuildUrlWithPage(request, 3);
            Assert.Equal("/list?order=name&page=3", result);
        }

        // ── BuildUrlWithPage (string) ──────────────────────────────────────────

        [Fact]
        public void BuildUrlWithPage_String_Page1_RemovesPageParam()
        {
            var result = UrlBuilder.BuildUrlWithPage("/list?page=5", 1);
            Assert.Equal("/list", result);
        }

        [Fact]
        public void BuildUrlWithPage_String_PageGreaterThan1_SetsPageParam()
        {
            var result = UrlBuilder.BuildUrlWithPage("/list", 4);
            Assert.Equal("/list?page=4", result);
        }

        [Fact]
        public void BuildUrlWithPage_String_PreservesOtherParams()
        {
            var result = UrlBuilder.BuildUrlWithPage("/list?order=name", 2);
            Assert.Equal("/list?order=name&page=2", result);
        }

        [Fact]
        public void BuildUrlWithPage_String_NoQueryString_Page1_ReturnsPathOnly()
        {
            var result = UrlBuilder.BuildUrlWithPage("/list", 1);
            Assert.Equal("/list", result);
        }

        // ── BuildUrlWithPage (Uri) ─────────────────────────────────────────────

        [Fact]
        public void BuildUrlWithPage_Uri_SetsPageParam()
        {
            var uri = new Uri("/list?order=date", UriKind.Relative);
            var result = UrlBuilder.BuildUrlWithPage(uri, 3);
            Assert.Equal("/list?order=date&page=3", result);
        }

        [Fact]
        public void BuildUrlWithPage_Uri_Page1_RemovesPageParam()
        {
            var uri = new Uri("/list?page=2", UriKind.Relative);
            var result = UrlBuilder.BuildUrlWithPage(uri, 1);
            Assert.Equal("/list", result);
        }

        // ── BuildUrlWithOrder (HttpRequest) ───────────────────────────────────

        [Fact]
        public void BuildUrlWithOrder_HttpRequest_NewColumn_SetsOrder()
        {
            var request = BuildRequest("/list");
            var result = UrlBuilder.BuildUrlWithOrder(request, "name");
            Assert.Equal("/list?order=name", result);
        }

        [Fact]
        public void BuildUrlWithOrder_HttpRequest_SameColumn_AppendsDesc()
        {
            var request = BuildRequest("/list", "?order=name");
            var result = UrlBuilder.BuildUrlWithOrder(request, "name");
            Assert.Equal("/list?order=name%20desc", result);
        }

        [Fact]
        public void BuildUrlWithOrder_HttpRequest_DescColumn_RemovesDesc()
        {
            var request = BuildRequest("/list", "?order=name%20desc");
            var result = UrlBuilder.BuildUrlWithOrder(request, "name");
            Assert.Equal("/list?order=name", result);
        }

        // ── BuildUrlWithOrder (string) ─────────────────────────────────────────

        [Fact]
        public void BuildUrlWithOrder_String_NewColumn_SetsOrder()
        {
            var result = UrlBuilder.BuildUrlWithOrder("/list", "name");
            Assert.Equal("/list?order=name", result);
        }

        [Fact]
        public void BuildUrlWithOrder_String_SameColumn_AppendsDesc()
        {
            var result = UrlBuilder.BuildUrlWithOrder("/list?order=name", "name");
            Assert.Equal("/list?order=name%20desc", result);
        }

        [Fact]
        public void BuildUrlWithOrder_String_DescColumn_RemovesDesc()
        {
            var result = UrlBuilder.BuildUrlWithOrder("/list?order=name%20desc", "name");
            Assert.Equal("/list?order=name", result);
        }

        [Fact]
        public void BuildUrlWithOrder_String_CustomOrderName()
        {
            var result = UrlBuilder.BuildUrlWithOrder("/list", "date", orderName: "sort");
            Assert.Equal("/list?sort=date", result);
        }

        // ── BuildUrlWithOrder (Uri) ────────────────────────────────────────────

        [Fact]
        public void BuildUrlWithOrder_Uri_NewColumn_SetsOrder()
        {
            var uri = new Uri("/list", UriKind.Relative);
            var result = UrlBuilder.BuildUrlWithOrder(uri, "name");
            Assert.Equal("/list?order=name", result);
        }

        [Fact]
        public void BuildUrlWithOrder_Uri_SameColumn_AppendsDesc()
        {
            var uri = new Uri("/list?order=name", UriKind.Relative);
            var result = UrlBuilder.BuildUrlWithOrder(uri, "name");
            Assert.Equal("/list?order=name%20desc", result);
        }
    }
}
