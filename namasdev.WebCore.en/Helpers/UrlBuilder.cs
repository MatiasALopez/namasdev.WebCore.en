using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace namasdev.WebCore.Helpers
{
    public class UrlBuilder
    {
        public const string ORDER_SUFIX_DESC = " desc";
        public const string ORDER_NAME = "order";
        public const string PAGE_NAME = "page";

        public static string BuildUrlWithPage(HttpRequest request, int page)
            => BuildUrlWithParameter(request, PAGE_NAME, page > 1 ? page.ToString() : null);

        public static string BuildUrlWithOrder(HttpRequest request, string order,
            bool applyOrderDescToFirstElementOnly = false,
            string orderName = ORDER_NAME)
        {
            var qs = QueryHelpers.ParseQuery(request.QueryString.Value);

            qs.TryGetValue(orderName, out StringValues currentOrderValues);
            string? currentOrder = currentOrderValues.FirstOrDefault();

            order = BuildOrderExpression(order, currentOrder, applyOrderDescToFirstElementOnly);

            return BuildUrlWithParameter(request, orderName, order);
        }

        public static string BuildOrderExpression(string order, string? currentOrder,
            bool applyOrderDescToFirstElementOnly = false)
        {
            string descExpression = BuildDescExpression(order, applyOrderDescToFirstElementOnly);

            if (string.Equals(currentOrder, descExpression))
                return order;           // desc → asc

            if (string.Equals(order, currentOrder))
                return descExpression;  // asc → desc

            return order;               // new column
        }

        public static string BuildUrlWithParameter(HttpRequest request, string paramName, string? paramValue)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var qs = QueryHelpers.ParseQuery(request.QueryString.Value);
            if (!string.IsNullOrWhiteSpace(paramValue))
            {
                qs[paramName] = paramValue;
            }
            else
            {
                qs.Remove(paramName);
            }

            var queryString = QueryString.Create(
                qs.Select(kvp => KeyValuePair.Create(kvp.Key, (string?)kvp.Value.ToString())));
            return request.Path + queryString.Value;
        }

        public static string BuildAbsoluteUrl(HttpRequest request, string relativeUrl)
        {
            return $"{request.Scheme}://{request.Host}{relativeUrl}";
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
    }
}
