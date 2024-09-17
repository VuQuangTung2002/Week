using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Week.Areas.Admin.Attributes;
using Week.Models;
using X.PagedList;
namespace Week.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class RoleController : Controller
    {
        public readonly MyDbConnect db;
        public RoleController(MyDbConnect _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
           
            return View();
        }
        public IActionResult List_Role(int ? page,int UserId)
        {
                int page_size = 5;
                int page_number = page ?? 1;
                // liet ke ra tat ca cac Id cua du lieu ra man hinh
                List<ItemRole> role = db.Roles.Where(s=> s.Id == UserId).OrderByDescending(s=> s.RoleId).ToList();
            ViewBag.User = UserId;
            return View("List_Role", role.ToPagedList(page_number, page_size));   
        }
        public IActionResult List_Permission(int RoleId,int ? page)
        {
            int page_size = 7;
            int page_number = page ?? 1;
            //var ds = (from role in db.Roles
            //          join rolepermission in db.RolePermisson on role.RoleId equals rolepermission.RoleId
            //          join permission in db.Permisson on rolepermission.PermissionId equals permission.PermissionId
            //          where role.RoleId == RoleId
            //          select permission).Distinct().ToList();
            List<ItemRolePermisson> k = db.RolePermisson.Where(s => s.RoleId == RoleId).ToList();
            List<ItemPermission> p = new List<ItemPermission>();

            foreach(var x in k)
            {
                ItemPermission per = db.Permisson.Where(s=> s.PermissionId ==  x.PermissionId).FirstOrDefault();
                p.Add(per);
            }
            ViewBag.permission = RoleId;
            //return Ok(RoleId.ToString());
            return View("List_Permission", p.ToPagedList(page_number, page_size));
        }
        public IActionResult List_All_User(int? page)
        {
            int page_size = 7;
            int page_number = page ?? 1;
            List<ItemUsers> x = db.Users.OrderByDescending(s => s.Id).ToList();
            return View("List_All_User", x.ToPagedList(page_number, page_size));
        }
        public IActionResult CreateRole(int UserId)
        {
            // liet ke ra tat ca cac Id cua du lieu ra man hinh
            ItemUsers s = db.Users.Where(s=> s.Id == UserId).FirstOrDefault();
            ViewBag.actionRole = "/Admin/Role/CreateRolePost?UserId=" + UserId;
            ViewBag.User = UserId;
            return View("CreateUpdateRole",s);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateRolePost(IFormCollection fc,int UserId)
        {
            string NameRole = !string.IsNullOrEmpty(fc["NameRole"].ToString()) ? fc["NameRole"].ToString() : "";
            try
            {
                db.Add_with_role(UserId, NameRole);
                string script = "<script>alert('Bạn Đã Thêm Mới Quyền Thành Công!');" +
                            "window.location.href = '" + Url.Action("Index", "Home") + "';</script>";
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                string script = "<script>alert('Bạn Đã Thất Bại Trong Việc Thêm Mới Quyền!');" +
                            "window.location.href = '" + Url.Action("Index", "Home") + "';</script>";
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }
        }




    }
}
