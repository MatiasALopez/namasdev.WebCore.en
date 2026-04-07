using Microsoft.AspNetCore.Mvc.Rendering;

using namasdev.WebCore.Helpers;

namespace namasdev.WebCore.Tests.Helpers
{
    public class ListHelperTests
    {
        // ── GetRolesSelectList ─────────────────────────────────────────────────

        [Fact]
        public void GetRolesSelectList_ReturnsItemPerRole()
        {
            var roles = new[] { "Admin", "User", "Guest" };
            var list = ListHelper.GetRolesSelectList(roles);
            var items = list.Cast<SelectListItem>().ToList();
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public void GetRolesSelectList_WithSelectedValue_MarksSelected()
        {
            var roles = new[] { "Admin", "User" };
            var list = ListHelper.GetRolesSelectList(roles, selectedValue: "Admin");
            Assert.Equal("Admin", list.SelectedValues?.Cast<string>().FirstOrDefault());
        }

        // ── GetRolesSelectListItems ────────────────────────────────────────────

        [Fact]
        public void GetRolesSelectListItems_SelectedValues_MarksCorrectItems()
        {
            var roles = new[] { "Admin", "User", "Guest" };
            var items = ListHelper.GetRolesSelectListItems(roles, new[] { "Admin", "Guest" }).ToList();

            Assert.True(items.First(i => i.Value == "Admin").Selected);
            Assert.False(items.First(i => i.Value == "User").Selected);
            Assert.True(items.First(i => i.Value == "Guest").Selected);
        }

        // ── GetYesNoSelectList ─────────────────────────────────────────────────

        [Fact]
        public void GetYesNoSelectList_ReturnsTwoItems()
        {
            var list = ListHelper.GetYesNoSelectList();
            Assert.Equal(2, list.Count());
        }

        // ── GetYearsSelectList ─────────────────────────────────────────────────

        [Fact]
        public void GetYearsSelectList_Range_ReturnsCorrectCount()
        {
            var list = ListHelper.GetYearsSelectList(2020, 2023);
            Assert.Equal(4, list.Count());
        }

        [Fact]
        public void GetYearsSelectList_SameFromAndTo_ReturnsSingleItem()
        {
            var list = ListHelper.GetYearsSelectList(2022, 2022);
            Assert.Single(list);
        }

        // ── GetQuartersSelectList ─────────────────────────────────────────────

        [Fact]
        public void GetQuartersSelectList_ReturnsFourItems()
        {
            var list = ListHelper.GetQuartersSelectList();
            Assert.Equal(4, list.Count());
        }

        [Fact]
        public void GetQuartersSelectList_ItemsLabeledQ1ToQ4()
        {
            var list = ListHelper.GetQuartersSelectList();
            var texts = list.Cast<SelectListItem>().Select(i => i.Text).ToList();
            Assert.Contains("Q1", texts);
            Assert.Contains("Q4", texts);
        }

        // ── GetMonthSelectList ────────────────────────────────────────────────

        [Fact]
        public void GetMonthSelectList_Returns12Items()
        {
            var list = ListHelper.GetMonthSelectList();
            Assert.Equal(12, list.Count());
        }

        // ── GetEmptySelectList ────────────────────────────────────────────────

        [Fact]
        public void GetEmptySelectList_ReturnsNoItems()
        {
            var list = ListHelper.GetEmptySelectList();
            Assert.Empty(list);
        }

        // ── CreateSelectListFromList ──────────────────────────────────────────

        [Fact]
        public void CreateSelectListFromList_NullItems_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => ListHelper.CreateSelectListFromList<string>(null!, s => new SelectListItem { Value = s, Text = s }));
        }

        [Fact]
        public void CreateSelectListFromList_NullSelector_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => ListHelper.CreateSelectListFromList(new[] { "a" }, (Func<string, SelectListItem>)null!));
        }

        [Fact]
        public void CreateSelectListFromItems_NullItems_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => ListHelper.CreateSelectListFromItems(null!));
        }

        // ── GetTimesSelectListItems ───────────────────────────────────────────

        [Fact]
        public void GetTimesSelectListItems_DefaultInterval_Returns48Items()
        {
            // Default interval is 30 min → 48 slots per day
            var items = ListHelper.GetTimesSelectListItems();
            Assert.Equal(48, items.Count());
        }

        [Fact]
        public void GetTimesSelectListItems_HourInterval_Returns24Items()
        {
            var items = ListHelper.GetTimesSelectListItems(TimeSpan.FromHours(1));
            Assert.Equal(24, items.Count());
        }
    }
}
