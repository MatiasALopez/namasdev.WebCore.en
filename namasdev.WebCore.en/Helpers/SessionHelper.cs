using Microsoft.AspNetCore.Http;

namespace namasdev.WebCore.Helpers
{
    public class SessionHelper
    {
        private readonly ISession _session;

        public SessionHelper(ISession session)
        {
            _session = session;
        }

        public bool GetBool(string key)
        {
            return _session.GetString(key) == "true";
        }

        public void SetBool(string key, bool value)
        {
            if (value)
            {
                _session.SetString(key, "true");
            }
            else
            {
                _session.Remove(key);
            }
        }

        public string? GetString(string key)
        {
            return _session.GetString(key);
        }

        public void SetString(string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _session.SetString(key, value);
            }
            else
            {
                _session.Remove(key);
            }
        }
    }
}
