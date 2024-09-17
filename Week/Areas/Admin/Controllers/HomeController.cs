using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bc = BCrypt.Net;
using Week.Areas.Admin.Attributes;
using Week.Models;
namespace Week.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class HomeController : Controller
    {
        //[Authorize(Policy = "AdminOnly")]
        private readonly MyDbConnect _db;

        public HomeController(MyDbConnect db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                string useremail = HttpContext.Session.GetString("user_email");
                if (string.IsNullOrEmpty(useremail))
                {
                    return Unauthorized("User email not found in session");
                }
                //return new JsonResult(permissions);
                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
            //return View();
        }
    }
}
