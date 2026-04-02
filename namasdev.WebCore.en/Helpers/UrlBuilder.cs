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

        public static string BuildUrlWithPage(Uri url, int page)
        {
            return BuildUrlWithParameter(url, PAGE_NAME, page.ToString());
        }

        public static string BuildUrlWithOrder(Uri url, string order,
            bool applyOrderDescToFirstElementOnly = false,
            string orderName = ORDER_NAME)
        {
            var qs = QueryHelpers.ParseQuery(url.Query);

            qs.TryGetValue(orderName, out StringValues currentOrderValues);
            string? currentOrder = currentOrderValues.FirstOrDefault();

            order = BuildOrderExpression(order, currentOrder,
                applyOrderDescToFirstElementOnly);

            return BuildUrlWithParameter(url, orderName, order);
        }

        public static string BuildOrderExpression(string order, string? currentOrder,
            bool applyOrderDescToFirstElementOnly = false)
        {
            if (string.Equals(order, currentOrder))
            {
                int separatorIndex = order.IndexOf(',');
                if (separatorIndex >= 0)
                {
                    if (applyOrderDescToFirstElementOnly)
                    {
                        order = order.Insert(separatorIndex, ORDER_SUFIX_DESC);
                    }
                    else
                    {
                        order = order.Replace(",", ORDER_SUFIX_DESC + ",") + ORDER_SUFIX_DESC;
                    }
                }
                else
                {
                    order = order + ORDER_SUFIX_DESC;
                }
            }

            return order;
        }

        public static string BuildUrlWithParameter(Uri url, string paramName, string? paramValue)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            var qs = QueryHelpers.ParseQuery(url.Query);
            if (!string.IsNullOrWhiteSpace(paramValue))
            {
                qs[paramName] = paramValue;
            }
            else
            {
                qs.Remove(paramName);
            }

            string path = url.GetLeftPart(UriPartial.Path);
            var queryString = QueryString.Create(
                qs.Select(kvp => KeyValuePair.Create(kvp.Key, (string?)kvp.Value.ToString())));
            return path + queryString.Value;
        }

        public static string BuildAbsoluteUrl(HttpRequest request, string relativeUrl)
        {
            return $"{request.Scheme}://{request.Host}{relativeUrl}";
        }
    }
}
