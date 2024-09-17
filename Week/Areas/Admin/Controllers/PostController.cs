using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using Week.Areas.Admin.Attributes;
using Week.Models;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Week.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public readonly MyDbConnect db;

        public PostController(MyDbConnect _db)
        {
            db = _db;
        }
        //[RequirePermission(Permissions.User.List_Task)]
        //[RequirePermission(Permissions.Admin.List_Task)]
        public async Task<IActionResult> List_Post(int? page)
        {
            KiemTra kt = new KiemTra(db);
            var ds = kt.List(HttpContext);
            if (ds.Contains("Post.List_Post"))
            {
                int page_number = page ?? 1;
                int page_size = 5;
                var list = db.Posts.OrderByDescending(s => s.PostId);
                ViewBag.errorSelectTask = TempData["errorSelectTask"];
                ViewBag.create = TempData["createMessage"];
                return View("List_Post", await list.ToPagedListAsync(page_number, page_size));
            }
            else
            {
                string script = "<script>alert('Bạn không có đủ quyền vui lòng liên hệ với Admin để biết thêm thông tin!');" + "window.location.href = '" + Url.Action("Index", "Home") + "';</script>";
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }

        }


        // chuc nang tim kiem  Task
        //[RequirePermission(Permissions.Admin.Create)]
        public IActionResult Create()
        {
            KiemTra kt = new KiemTra(db);
            var ds = kt.List(HttpContext);
            if (ds.Contains("Post.Create"))
            {
                ViewBag.task = "/Admin/Post/CreatePost";
                return View("CreatePost");
            }
            else
            {
                string script = "<script>alert('Bạn không có đủ quyền vui lòng liên hệ với Admin để biết thêm thông tin!');" + "window.location.href = '" + Url.Action("List_Post", "Post") + "';</script>";
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }
        }
        //[RequirePermission(Permissions.Admin.CreatePost)]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreatePost(IFormCollection fc)
        {
            KiemTra kt = new KiemTra(db);
            var ds = kt.List(HttpContext);
            if (ds.Contains("Post.Create"))
            {
                var NoiDung = !string.IsNullOrEmpty(fc["NoiDung"].ToString().Trim()) ? fc["NoiDung"].ToString() : "";
                var TieuDe = !string.IsNullOrEmpty(fc["TieuDe"].ToString().Trim()) ? fc["TieuDe"].ToString() : "";
                DateTime NgayDang;
                if (!DateTime.TryParse(fc["NgayDang"].ToString().Trim(), out NgayDang))
                {
                    // Nếu không thể parse, bạn có thể trả về lỗi hoặc gán giá trị mặc định
                    NgayDang = DateTime.UtcNow; // hoặc bất kỳ giá trị nào bạn muốn
                }
                var UserName = !string.IsNullOrEmpty(fc["UserName"].ToString().Trim()) ? fc["UserName"].ToString() : "";
                var Image = !string.IsNullOrEmpty(fc["Image"].ToString().Trim()) ? fc["Image"].ToString() : "";
                var sanitizer = new Ganss.XSS.HtmlSanitizer();

                // Lọc HTML để chỉ giữ lại các thẻ hợp lệ
                var safeHtml = sanitizer.Sanitize(NoiDung);

                var UserId = db.Users.Where(s => s.UserName == UserName).FirstOrDefault();
                try
                {
                    ItemPosts cate = new ItemPosts();

                    cate.NoiDung = NoiDung;
                    cate.TieuDe = TieuDe;
                    cate.NgayDang = NgayDang;
                    cate.Id = UserId.Id;
                    string _FileName = "";
                    try
                    {
                        //lay ten file
                        _FileName = Request.Form.Files[0].FileName;
                    }
                    catch {; }
                    if (!string.IsNullOrEmpty(_FileName))
                    {
                        //upload anh moi
                        //lay thoi gian gan vao ten file -> tranh cac file co ten trung ten voi file se upload
                        var timestap = DateTime.Now.ToFileTime();
                        _FileName = timestap + "_" + _FileName;
                        //lay duong dan cua file
                        string _Path = Path.Combine(Directory.GetCurrentDirectory(), "~/Image/", _FileName);
                        //upload file
                        using (var stream = new FileStream(_Path, FileMode.Create))
                        {
                            Request.Form.Files[0].CopyTo(stream);
                        }
                    }
                    cate.Image = _FileName;
                    db.Posts.Add(cate);

                    db.SaveChanges();

                    TempData["createMessage"] = "Đã Tạo Mới Thành Công";
                }
                catch (Exception ex)
                {
                    TempData["createMessage"] = "Đã Thất Bại Trong Quá Trình Tạo Mới: " + ex.Message;
                }
                return RedirectToAction("List_Post", "Post");
            }
            else
            {
                string script = "<script>alert('Bạn không có đủ quyền vui lòng liên hệ với Admin để biết thêm thông tin!');" + "window.location.href = '" + Url.Action("List_Post", "Post") + "';</script>";
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }

        }
        //[RequirePermission(Permissions.Admin.Delete)]
        public IActionResult Delete(int? id)
        {
            KiemTra kt = new KiemTra(db);
            var ds = kt.List(HttpContext);
            if (ds.Contains("Post.Delete"))
            {
                int PostId = id ?? 0;
                ItemPosts task = db.Posts.Where(s => s.PostId == PostId).FirstOrDefault();
                db.Posts.Remove(task);
                db.SaveChanges();
                return RedirectToAction("List_Post", "Post");
            }
            else
            {
                string script = "<script>alert('Bạn không có đủ quyền vui lòng liên hệ với Admin để biết thêm thông tin!');" + "window.location.href = '" + Url.Action("List_Post", "Post") + "';</script>";
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }
        }
        public IActionResult Update(int? id)
        {
            KiemTra kt = new KiemTra(db);
            var ds = kt.List(HttpContext);
            if (ds.Contains("Post.Update"))
            {
                int _id = id ?? 0;
                ViewBag.task = "/Admin/Post/UpdatePost/" + _id;
                ViewBag.PostId = id;
                var x = db.Posts.Where(s => s.PostId == _id).FirstOrDefault();
                return View("CreatePost", x);
            }
            else
            {
                string script = "<script>alert('Bạn không có đủ quyền vui lòng liên hệ với Admin để biết thêm thông tin!');" + "window.location.href = '" + Url.Action("List_Post", "Post") + "';</script>";
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult UpdatePost(int id, IFormCollection fc)
        {
            KiemTra kt = new KiemTra(db);
            var ds = kt.List(HttpContext);
            if (ds.Contains("Post.Update"))
            {
                var NoiDung = !string.IsNullOrEmpty(fc["NoiDung"].ToString().Trim()) ? fc["NoiDung"].ToString() : "";
                var TieuDe = !string.IsNullOrEmpty(fc["TieuDe"].ToString().Trim()) ? fc["TieuDe"].ToString() : "";
                var NgayDang = !string.IsNullOrEmpty(fc["NgayDang"].ToString().Trim()) ? fc["NgayDang"].ToString() : "";
                var UserName = !string.IsNullOrEmpty(fc["UserName"].ToString().Trim()) ? fc["UserName"].ToString() : "";
                var Image = !string.IsNullOrEmpty(fc["Image"].ToString().Trim()) ? fc["Image"].ToString() : "";
                var UserId = db.Users.Where(s => s.UserName == UserName).FirstOrDefault();

                var x = db.Posts.Where(s => s.PostId == id).FirstOrDefault();
                if (x != null)
                {
                    x.NoiDung = NoiDung;
                    x.TieuDe = TieuDe;
                    x.NgayDang = DateTime.Parse(NgayDang).ToUniversalTime();
                    x.Id = UserId.Id;
                    string _FileName = "";
                    try
                    {
                        //lay ten file
                        _FileName = Request.Form.Files[0].FileName;
                    }
                    catch {; }
                    if (!string.IsNullOrEmpty(_FileName))
                    {
                        //xoa anh cu
                        if (x.Image != null && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", x.Image)))
                        {
                            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", x.Image));
                        }
                        //upload anh moi
                        //lay thoi gian gan vao ten file -> tranh cac file co ten trung ten voi file se upload
                        var timestap = DateTime.UtcNow.ToFileTime();
                        _FileName = timestap + "_" + _FileName;
                        //lay duong dan cua file
                        string _Path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload/Adv", _FileName);
                        //upload file
                        using (var stream = new FileStream(_Path, FileMode.Create))
                        {
                            Request.Form.Files[0].CopyTo(stream);
                        }
                        //update gia tri vao cot Photo trong csdl
                        x.Image = _FileName;
                    }
                    TempData["UpdateTask"] = "Cập nhật thành công";
                    db.SaveChanges();
                }
                else
                {
                    TempData["UpdateTask"] = "Cập nhật Task " + x.TieuDe + " Thất bại, Vui lòng cập nhật lại!";

                }


                return RedirectToAction("List_Post", "Post");
            }
            else
            {
                string script = "<script>alert('Bạn không có đủ quyền vui lòng liên hệ với Admin để biết thêm thông tin!');" + "window.location.href = '" + Url.Action("List_Post", "Post") + "';</script>";
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }
        }
        //[RequirePermission(Permissions.Admin.search)]
        //[RequirePermission(Permissions.User.search)]
        public async Task<IActionResult> search(int? page)
        {
            KiemTra kt = new KiemTra(db);
            var ds = kt.List(HttpContext);
            if (!ds.Contains("Post.search"))
            {
                string script = "<script>alert('Bạn không có đủ quyền vui lòng liên hệ với Admin để biết thêm thông tin!');" + "window.location.href = '" + Url.Action("List_Post", "Post") + "';</script>";
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }

            IQueryable<ItemPosts> query = db.Posts;

            if (Request.Query.TryGetValue("Searchkey", out var searchKeyValue) &&
                Request.Query.TryGetValue("select", out var selectValue))
            {
                string searchKey = searchKeyValue.ToString();
                string select = selectValue.ToString();

                switch (select)
                {
                    case "TieuDe":
                        query = query.Where(s => s.TieuDe.Contains(searchKey));
                        break;
                    case "NoiDung":
                        query = query.Where(s => s.NoiDung.Contains(searchKey));
                        break;
                    case "NguoiDang":
                        var user = await db.Users.FirstOrDefaultAsync(s => s.UserName.Contains(searchKey));
                        if (user != null)
                        {
                            query = query.Where(s => s.Id == user.Id);
                        }
                        else
                        {
                            query = query.Where(s => false);
                            TempData["errorSelectTask"] = "Không tìm thấy người dùng với tên đã nhập.";
                        }
                        break;
                    default:
                        TempData["errorSelectTask"] = "Loại tìm kiếm không hợp lệ.";
                        break;
                }

                ViewBag.Searchkey = searchKey;
                ViewBag.select = select;
            }

            query = query.OrderByDescending(s => s.PostId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var suggestion = await db.Posts.Take(4).Select(t => new { t.TieuDe, t.PostId }).ToListAsync();
                return Json(suggestion);
            }

            int pageNumber = page ?? 1;
            int pageSize = 3;
            var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);

            if (pagedList.Count == 0)
            {
                TempData["errorSelectTask"] = TempData["errorSelectTask"] ?? "Không có kết quả nào được tìm kiếm, Vui lòng thao tác lại";
                HttpContext.Session.SetString("post", TempData["errorSelectTask"].ToString());
                return RedirectToAction("List_Post", "Post");
            }

            ViewBag.errorSelectTask = TempData["errorSelectTask"];
            return View("search", pagedList);
        }
        //return Ok(TempData["errorSelectTask"]);
    }
    //[RequirePermission(Permissions.Admin.Update)]
}
