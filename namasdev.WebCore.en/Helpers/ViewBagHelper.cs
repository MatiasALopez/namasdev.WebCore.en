namespace namasdev.WebCore.Helpers
{
    public class ViewBagHelper
    {
        public static void SetMessageSuccess(dynamic viewBag, params string[] messages)
        {
            viewBag.MessageSuccess = FormatMessages(messages);
        }

        public static string[]? GetMessageSuccess(dynamic viewBag)
        {
            return viewBag.MessageSuccess as string[];
        }

        public static void SetMessageInfo(dynamic viewBag, params string[] messages)
        {
            viewBag.MessageInfo = FormatMessages(messages);
        }

        public static string[]? GetMessageInfo(dynamic viewBag)
        {
            return viewBag.MessageInfo as string[];
        }

        public static void SetMessageWarning(dynamic viewBag, params string[] messages)
        {
            viewBag.MessageWarning = FormatMessages(messages);
        }

        public static string[]? GetMessageWarning(dynamic viewBag)
        {
            return viewBag.MessageWarning as string[];
        }

        public static void SetMessageError(dynamic viewBag, params string[] messages)
        {
            viewBag.MessageError = FormatMessages(messages);
        }

        public static string[]? GetMessageError(dynamic viewBag)
        {
            return viewBag.MessageError as string[];
        }

        private static string[]? FormatMessages(IEnumerable<string> messages)
        {
            return messages != null && messages.Any()
                ? messages.Select(FormatMessage).ToArray()
                : null;
        }

        private static string FormatMessage(string message)
        {
            return Core.Types.Formatter.Html(message);
        }
    }
}
