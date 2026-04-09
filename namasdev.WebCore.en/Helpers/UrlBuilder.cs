using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

using namasdev.Core.Validation;

namespace namasdev.WebCore.Helpers
{
    public class UrlBuilder
    {
        public const string ORDER_SUFIX_DESC = " desc";
        public const string ORDER_NAME = "order";
        public const string PAGE_NAME = "page";

        public static string BuildUrlWithPage(HttpRequest request, int page)
            => BuildUrlWithParameter(request, PAGE_NAME, page > 1 ? page.ToString() : null);

        public static string BuildUrlWithPage(string url, int page)
            => BuildUrlWithParameter(url, PAGE_NAME, page > 1 ? page.ToString() : null);

        public static string BuildUrlWithPage(Uri uri, int page)
            => BuildUrlWithPage(uri.OriginalString, page);

        public static string BuildUrlWithOrder(HttpRequest request, string order,
            bool applyOrderDescToFirstElementOnly = false,
            string orderName = ORDER_NAME)
        {
            Validator.ValidateRequiredArgumentAndThrow(request, nameof(request));
            return BuildUrlWithOrder(
                request.Path + request.QueryString.Value,
                order, applyOrderDescToFirstElementOnly, orderName);
        }

        public static string BuildUrlWithOrder(string url, string order,
            bool applyOrderDescToFirstElementOnly = false,
            string orderName = ORDER_NAME)
        {
            var (_, rawQuery) = SplitUrl(url);
            string? currentOrder = GetQueryParamValue(rawQuery, orderName);
            order = BuildOrderExpression(order, currentOrder, applyOrderDescToFirstElementOnly);
            return BuildUrlWithParameter(url, orderName, order);
        }

        public static string BuildUrlWithOrder(Uri uri, string order,
            bool applyOrderDescToFirstElementOnly = false,
            string orderName = ORDER_NAME)
            => BuildUrlWithOrder(uri.OriginalString, order, applyOrderDescToFirstElementOnly, orderName);

        private static string BuildOrderExpression(string order, string? currentOrder,
            bool applyOrderDescToFirstElementOnly = false)
        {
            string descExpression = BuildDescExpression(order, applyOrderDescToFirstElementOnly);

            if (string.Equals(currentOrder, descExpression))
                return order;           // desc → asc

            if (string.Equals(order, currentOrder))
                return descExpression;  // asc → desc

            return order;               // new column
        }

        private static string BuildUrlWithParameter(HttpRequest request, string paramName, string? paramValue)
        {
            Validator.ValidateRequiredArgumentAndThrow(request, nameof(request));
            return BuildUrlWithParameter(request.Path + request.QueryString.Value, paramName, paramValue);
        }

        private static string BuildAbsoluteUrl(HttpRequest request, string relativeUrl)
            => $"{request.Scheme}://{request.Host}{relativeUrl}";

        private static string BuildUrlWithParameter(string url, string paramName, string? paramValue)
        {
            var (path, rawQuery) = SplitUrl(url);
            var qs = QueryHelpers.ParseQuery(rawQuery);

            if (!string.IsNullOrWhiteSpace(paramValue))
                qs[paramName] = paramValue;
            else
                qs.Remove(paramName);

            var queryString = QueryString.Create(
                qs.Select(kvp => KeyValuePair.Create(kvp.Key, (string?)kvp.Value.ToString())));
            return path + queryString.Value;
        }

        private static string BuildDescExpression(string order, bool applyOrderDescToFirstElementOnly)
        {
            int separatorIndex = order.IndexOf(',');
            if (separatorIndex >= 0)
            {
                return applyOrderDescToFirstElementOnly
                    ? order.Insert(separatorIndex, ORDER_SUFIX_DESC)
                    : order.Replace(",", ORDER_SUFIX_DESC + ",") + ORDER_SUFIX_DESC;
            }
            return order + ORDER_SUFIX_DESC;
        }

        private static (string path, string? rawQuery) SplitUrl(string url)
        {
            int qi = url.IndexOf('?');
            return qi >= 0
                ? (url[..qi], url[qi..])
                : (url, null);
        }

        private static string? GetQueryParamValue(string? rawQuery, string paramName)
        {
            var qs = QueryHelpers.ParseQuery(rawQuery);
            qs.TryGetValue(paramName, out StringValues values);
            return values.FirstOrDefault();
        }
    }
}
