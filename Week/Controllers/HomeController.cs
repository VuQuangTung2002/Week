using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Week.Models;
using Bc = BCrypt.Net;
namespace Week.Controllers
{
    public class HomeController : Controller
    {
        public readonly MyDbConnect db;
        public HomeController(MyDbConnect db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            List<ItemUsers> x = db.Users.ToList();
            var json = JsonConvert.SerializeObject(x);
          
            return Ok(json);
            //return View(json);
        }
    }
}
