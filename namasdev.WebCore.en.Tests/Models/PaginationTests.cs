using namasdev.WebCore.Models;

namespace namasdev.WebCore.Tests.Models
{
    public class PaginationTests
    {
        // ── Constructor ────────────────────────────────────────────────────────

        [Fact]
        public void Constructor_SetsProperties()
        {
            var p = new Pagination(2, 10, 35, 4);

            Assert.Equal(2, p.Page);
            Assert.Equal(10, p.PageItemCount);
            Assert.Equal(35, p.TotalItemCount);
            Assert.Equal(4, p.TotalPageCount);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_InvalidPage_DefaultsTo1(int page)
        {
            var p = new Pagination(page, 10, 50, 5);
            Assert.Equal(1, p.Page);
        }

        // ── IsSinglePage ───────────────────────────────────────────────────────

        [Fact]
        public void IsSinglePage_WhenTotalPageCountIs1_ReturnsTrue()
        {
            var p = new Pagination(1, 10, 5, 1);
            Assert.True(p.IsSinglePage);
        }

        [Fact]
        public void IsSinglePage_WhenTotalPageCountGreaterThan1_ReturnsFalse()
        {
            var p = new Pagination(1, 10, 20, 2);
            Assert.False(p.IsSinglePage);
        }

        // ── PreviousPageAvailable ──────────────────────────────────────────────

        [Fact]
        public void PreviousPageAvailable_OnFirstPage_ReturnsFalse()
        {
            var p = new Pagination(1, 10, 30, 3);
            Assert.False(p.PreviousPageAvailable);
        }

        [Fact]
        public void PreviousPageAvailable_OnSecondPage_ReturnsTrue()
        {
            var p = new Pagination(2, 10, 30, 3);
            Assert.True(p.PreviousPageAvailable);
        }

        // ── NextPageAvailable ─────────────────────────────────────────────────

        [Fact]
        public void NextPageAvailable_OnLastPage_ReturnsFalse()
        {
            var p = new Pagination(3, 10, 30, 3);
            Assert.False(p.NextPageAvailable);
        }

        [Fact]
        public void NextPageAvailable_OnMiddlePage_ReturnsTrue()
        {
            var p = new Pagination(2, 10, 30, 3);
            Assert.True(p.NextPageAvailable);
        }

        // ── PreviousPage ──────────────────────────────────────────────────────

        [Fact]
        public void PreviousPage_OnPage3_Returns2()
        {
            var p = new Pagination(3, 10, 30, 3);
            Assert.Equal(2, p.PreviousPage);
        }

        [Fact]
        public void PreviousPage_OnFirstPage_Returns1()
        {
            var p = new Pagination(1, 10, 30, 3);
            Assert.Equal(1, p.PreviousPage);
        }

        // ── NextPage ──────────────────────────────────────────────────────────

        [Fact]
        public void NextPage_OnPage2_Returns3()
        {
            var p = new Pagination(2, 10, 30, 3);
            Assert.Equal(3, p.NextPage);
        }

        [Fact]
        public void NextPage_OnLastPage_ReturnsTotalPageCount()
        {
            var p = new Pagination(3, 10, 30, 3);
            Assert.Equal(3, p.NextPage);
        }
    }
}
