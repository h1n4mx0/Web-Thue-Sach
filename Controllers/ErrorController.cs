using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.Controllers
{
    public class ErrorController : Controller
    {
        // Xử lý lỗi chung
        [Route("Error/{statusCode}")]
        public IActionResult HandleError(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("404");
            }
            return View("AnotherError");
        }

        [Route("Error/AccessDenied")]
        public IActionResult AccessDenied(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("AccessDenied");
        }
    }
}
