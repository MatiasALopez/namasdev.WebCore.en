using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using namasdev.WebCore.Helpers;

namespace namasdev.WebCore.Controllers
{
    public class ControllerBase : Controller
    {
        private ControllerHelper? _controllerHelper;
        protected ControllerHelper ControllerHelper
        {
            get { return _controllerHelper ??= new ControllerHelper(this); }
        }

        private UserHelper? _userHelper;
        protected UserHelper UserHelper
        {
            get { return _userHelper ??= new UserHelper(this.HttpContext); }
        }

        private SessionHelper? _sessionHelper;
        protected SessionHelper SessionHelper
        {
            get { return _sessionHelper ??= new SessionHelper(this.HttpContext.Session); }
        }

        private string? _userId;
        protected string? UserId
        {
            get { return _userId ??= UserHelper.UserId; }
        }

        private string? _userName;
        protected string? UserName
        {
            get { return _userName ??= UserHelper.UserName; }
        }

        public JsonResult CreateJsonResultOk(string? message = null)
        {
            return CreateJsonResult(ok: true, message: message);
        }

        public JsonResult CreateJsonResultError(string message)
        {
            return CreateJsonResult(ok: false, message: message);
        }

        private JsonResult CreateJsonResult(bool ok, string? message)
        {
            return Json(new { ok, message });
        }

        public async Task SignOutAndClearSession()
        {
            await HttpContext.SignOutAsync();

            HttpContext.Session.Clear();
        }
    }
}
