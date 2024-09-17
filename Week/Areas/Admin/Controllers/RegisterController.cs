using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Week.Models;
using BC = BCrypt.Net;
namespace Week.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RegisterController : Controller
    {
        public MyDbConnect db;
        public RegisterController(MyDbConnect db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            ViewBag.account = "/Admin/Register/RegisterPost";
            ViewBag.register = TempData["register"];
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        //public IActionResult RegisterPost(IFormCollection fc)
        //{
        //    try {
        //        var name = !string.IsNullOrEmpty(fc["name"]) ? fc["name"].ToString().Trim() : "";
        //        var email = !string.IsNullOrEmpty(fc["email"]) ? fc["email"].ToString().Trim() : "";
        //        var password = !string.IsNullOrEmpty(fc["password"]) ? fc["password"].ToString().Trim() : "";
        //        ItemUsers x = new ItemUsers();
        //        x.UserName = name;
        //        x.Email = email;
        //        x.PasswordHash = BC.BCrypt.HashPassword(password);
        //        x.AccessFailedCount = 0;
        //        x.ConcurrencyStamp = "9491a763-745a-47c2-8d14-79acb0722d05";
        //        x.PhoneNumber = "0";
        //        x.PhoneNumberConfirmed = false;
        //        x.EmailConfirmed = false;
        //        x.LockoutEnabled = false;
        //        x.LockoutEnd = null;
        //        x.TwoFactorEnabled = false;
        //        x.NormalizedEmail = "";
        //        x.NormalizedUserName = "";
        //        x.SecurityStamp = "MGYZTW6AH4XKCBDFSHJH6DMSMOXSODTA";

        //        db.Users.Add(x);
        //        db.SaveChanges();

        //        // tu dong cap nhat lai role = user 
        //        ItemRole role = new ItemRole();
        //        role.Name = "User";
        //        role.ConcurrencyStamp = "123223";
        //        role.NormalizedName = "User";
        //        role.Id = Convert.ToInt32(x.Id);
        //        db.Roles.Add(role);
        //        db.SaveChanges();
        //        TempData["register"] = email+" Accessted Seccessfull";
        //        return RedirectToAction("Login", "Account");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["register"] = "No Create Register with error: "+ex.Message;
        //        return RedirectToAction("Index", "Register");
        //    }

        //    //return RedirectToAction("Index","Register");
        //}
        public IActionResult RegisterPost(IFormCollection fc)
        {
            try
            {
                var name = !string.IsNullOrEmpty(fc["name"]) ? fc["name"].ToString().Trim() : "";
                var email = !string.IsNullOrEmpty(fc["email"]) ? fc["email"].ToString().Trim() : "";
                var password = !string.IsNullOrEmpty(fc["password"]) ? fc["password"].ToString().Trim() : "";

                // Kiểm tra xem email đã tồn tại chưa
                if (db.Users.Any(u => u.Email == email))
                {
                    TempData["register"] = "Email đã tồn tại trong hệ thống.";
                    return RedirectToAction("Index", "Register");
                }

                db.AddUserWithRole(name, email, BC.BCrypt.HashPassword(password), "User");


                // Liên kết user với role


                TempData["register"] = email + " Registered Successfully";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                TempData["register"] = "Registration failed with error: " + ex.InnerException?.Message ?? ex.Message;
                return RedirectToAction("Index", "Register");
            }
        }

    }
}
