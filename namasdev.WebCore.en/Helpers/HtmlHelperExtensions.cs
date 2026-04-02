using System.Linq.Expressions;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using namasdev.WebCore.Models;
using namasdev.WebCore.Configuration;

namespace namasdev.WebCore.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent DisplayNameWithoutEncodingFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var expressionProvider = html.ViewContext.HttpContext.RequestServices
                .GetRequiredService<IModelExpressionProvider>();
            var modelExpression = expressionProvider.CreateModelExpression(html.ViewData, expression);
            string displayName = modelExpression.Metadata.DisplayName
                ?? modelExpression.Metadata.PropertyName
                ?? expression.Body.ToString().Split('.').Last();
            return new HtmlString(displayName);
        }

        public static IHtmlContent MessageSuccess(this IHtmlHelper html,
            bool isDismissable = false)
        {
            return AlertSuccess(html,
                messages: ViewBagHelper.GetMessageSuccess(html.ViewBag),
                isDismissable: isDismissable);
        }

        public static IHtmlContent MessageInfo(this IHtmlHelper html,
            bool isDismissable = false)
        {
            return AlertInfo(html,
                messages: ViewBagHelper.GetMessageInfo(html.ViewBag),
                isDismissable: isDismissable);
        }

        public static IHtmlContent MessageWarning(this IHtmlHelper html,
            bool isDismissable = false)
        {
            return AlertWarning(html,
                messages: ViewBagHelper.GetMessageWarning(html.ViewBag),
                isDismissable: isDismissable);
        }

        public static IHtmlContent MessageError(this IHtmlHelper html,
            bool isDismissable = false)
        {
            return AlertDanger(html,
                messages: ViewBagHelper.GetMessageError(html.ViewBag),
                isDismissable: isDismissable);
        }

        public static IHtmlContent AlertSuccess(this IHtmlHelper html, string message,
            bool isDismissable = false)
        {
            return AlertSuccess(html,
                messages: new[] { message },
                isDismissable: isDismissable);
        }

        public static IHtmlContent AlertSuccess(this IHtmlHelper html, IEnumerable<string>? messages,
            bool isDismissable = false)
        {
            return Alert(html,
                AlertType.Success,
                messages: messages?.ToArray(),
                isDismissable: isDismissable);
        }

        public static IHtmlContent AlertInfo(this IHtmlHelper html, string message,
            bool isDismissable = false)
        {
            return AlertInfo(html,
                messages: new[] { message },
                isDismissable: isDismissable);
        }

        public static IHtmlContent AlertInfo(this IHtmlHelper html, IEnumerable<string>? messages,
            bool isDismissable = false)
        {
            return Alert(html,
                AlertType.Info,
                messages: messages?.ToArray(),
                isDismissable: isDismissable);
        }

        public static IHtmlContent AlertWarning(this IHtmlHelper html, string message,
            bool isDismissable = false)
        {
            return AlertWarning(html,
                messages: new[] { message },
                isDismissable: isDismissable);
        }

        public static IHtmlContent AlertWarning(this IHtmlHelper html, IEnumerable<string>? messages,
            bool isDismissable = false)
        {
            return Alert(html,
                AlertType.Warning,
                messages: messages?.ToArray(),
                isDismissable: isDismissable);
        }

        public static IHtmlContent AlertDanger(this IHtmlHelper html, string message,
            bool isDismissable = false)
        {
            return AlertDanger(html,
                messages: new[] { message },
                isDismissable: isDismissable);
        }

        public static IHtmlContent AlertDanger(this IHtmlHelper html, IEnumerable<string>? messages,
           bool isDismissable = false)
        {
            return Alert(html,
                AlertType.Danger,
                messages: messages?.ToArray(),
                isDismissable: isDismissable);
        }

        public static IHtmlContent Alert(this IHtmlHelper html, AlertType type, string[]? messages,
            bool isDismissable = false)
        {
            if (messages == null || !messages.Any())
            {
                return new HtmlString("");
            }

            var bootstrapVersion = html.ViewContext.HttpContext.RequestServices
                .GetService<IOptions<WebCoreOptions>>()?.Value.BootstrapVersion
                ?? BootstrapVersion.V5;

            string alertCssClass = $"alert alert-{type.ToString().ToLower()}",
                dismissButton = string.Empty;
            if (isDismissable)
            {
                alertCssClass += " alert-dismissible fade show";
                dismissButton = bootstrapVersion == BootstrapVersion.V4
                    ? "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>"
                    : "<button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"alert\" aria-label=\"Close\"></button>";
            }
            return new HtmlString($"<div class=\"{alertCssClass}\" role=\"alert\">{Core.Types.Formatter.List(messages, "<br/>")}{dismissButton}</div>");
        }
    }
}
