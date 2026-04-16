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

        [Fact]
        public void BuildUrlWithOrder_Uri_DescColumn_RemovesDesc()
        {
            var uri = new Uri("/list?order=name%20desc", UriKind.Relative);
            var result = UrlBuilder.BuildUrlWithOrder(uri, "name");
            Assert.Equal("/list?order=name", result);
        }

        // ── BuildUrlWithOrder — preserves other params / multi-column ─────────

        [Fact]
        public void BuildUrlWithOrder_String_PreservesOtherParams()
        {
            var result = UrlBuilder.BuildUrlWithOrder("/list?page=2&order=name", "name");
            Assert.Equal("/list?page=2&order=name%20desc", result);
        }

        [Fact]
        public void BuildUrlWithOrder_String_DifferentColumn_ReplacesOrder()
        {
            var result = UrlBuilder.BuildUrlWithOrder("/list?order=name", "date");
            Assert.Equal("/list?order=date", result);
        }

        [Fact]
        public void BuildUrlWithOrder_String_MultiColumn_SameOrder_AppendsDescToAll()
        {
            var result = UrlBuilder.BuildUrlWithOrder("/list?order=name,date", "name,date");
            Assert.Equal("/list?order=name%20desc,date%20desc", result);
        }

        [Fact]
        public void BuildUrlWithOrder_String_MultiColumn_ApplyDescToFirstElementOnly_AppendsDescToFirst()
        {
            var result = UrlBuilder.BuildUrlWithOrder("/list?order=name,date", "name,date",
                applyOrderDescToFirstElementOnly: true);
            Assert.Equal("/list?order=name%20desc,date", result);
        }

        [Fact]
        public void BuildUrlWithOrder_String_MultiColumn_DescOrder_RemovesDesc()
        {
            var result = UrlBuilder.BuildUrlWithOrder("/list?order=name%20desc,date%20desc", "name,date");
            Assert.Equal("/list?order=name,date", result);
        }

        [Fact]
        public void BuildUrlWithOrder_HttpRequest_CustomOrderName()
        {
            var request = BuildRequest("/list", "?sort=name");
            var result = UrlBuilder.BuildUrlWithOrder(request, "name", orderName: "sort");
            Assert.Equal("/list?sort=name%20desc", result);
        }

        [Fact]
        public void BuildUrlWithOrder_HttpRequest_NullRequest_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => UrlBuilder.BuildUrlWithOrder((HttpRequest)null!, "name"));
        }

        // ── BuildOrderExpression ──────────────────────────────────────────────

        [Fact]
        public void BuildOrderExpression_NullCurrentOrder_ReturnsOrder()
        {
            var result = UrlBuilder.BuildOrderExpression("name", null);
            Assert.Equal("name", result);
        }

        [Fact]
        public void BuildOrderExpression_DifferentCurrentOrder_ReturnsOrder()
        {
            var result = UrlBuilder.BuildOrderExpression("name", "date");
            Assert.Equal("name", result);
        }

        [Fact]
        public void BuildOrderExpression_SameAsCurrentOrder_ReturnsDesc()
        {
            var result = UrlBuilder.BuildOrderExpression("name", "name");
            Assert.Equal("name desc", result);
        }

        [Fact]
        public void BuildOrderExpression_CurrentOrderIsDesc_ReturnsAsc()
        {
            var result = UrlBuilder.BuildOrderExpression("name", "name desc");
            Assert.Equal("name", result);
        }

        [Fact]
        public void BuildOrderExpression_MultiColumn_SameAsCurrent_AppendsDescToAll()
        {
            var result = UrlBuilder.BuildOrderExpression("name,date", "name,date");
            Assert.Equal("name desc,date desc", result);
        }

        [Fact]
        public void BuildOrderExpression_MultiColumn_SameAsCurrent_FirstOnly_AppendsDescToFirst()
        {
            var result = UrlBuilder.BuildOrderExpression("name,date", "name,date",
                applyOrderDescToFirstElementOnly: true);
            Assert.Equal("name desc,date", result);
        }

        [Fact]
        public void BuildOrderExpression_MultiColumn_CurrentIsAllDesc_ReturnsAsc()
        {
            var result = UrlBuilder.BuildOrderExpression("name,date", "name desc,date desc");
            Assert.Equal("name,date", result);
        }

        [Fact]
        public void BuildOrderExpression_MultiColumn_CurrentIsFirstOnlyDesc_ReturnsAsc()
        {
            var result = UrlBuilder.BuildOrderExpression("name,date", "name desc,date",
                applyOrderDescToFirstElementOnly: true);
            Assert.Equal("name,date", result);
        }
    }
}
