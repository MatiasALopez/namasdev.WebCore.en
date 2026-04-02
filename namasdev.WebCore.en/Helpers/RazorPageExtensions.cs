using Microsoft.AspNetCore.Mvc.Razor;

namespace namasdev.WebCore.Helpers
{
    public static class RazorPageExtensions
    {
        public static string[]? GetMessageSuccess(this RazorPage page)
        {
            return ViewBagHelper.GetMessageSuccess(page.ViewBag);
        }

        public static string[]? GetMessageInfo(this RazorPage page)
        {
            return ViewBagHelper.GetMessageInfo(page.ViewBag);
        }

        public static string[]? GetMessageWarning(this RazorPage page)
        {
            return ViewBagHelper.GetMessageWarning(page.ViewBag);
        }

        public static string[]? GetMessageError(this RazorPage page)
        {
            return ViewBagHelper.GetMessageError(page.ViewBag);
        }
    }
}
