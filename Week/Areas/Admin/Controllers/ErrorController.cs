using Microsoft.AspNetCore.Mvc;
using Week.Areas.Admin.Attributes;
namespace Week.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.pass = TempData["pass"];
            return View();
        }
    }
}
