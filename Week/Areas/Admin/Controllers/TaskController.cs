using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity;
using X.PagedList;
using Week.Models;
using System.Drawing.Printing;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Week.Areas.Admin.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using AngleSharp.Common;
namespace Week.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CheckLogin]
    public class TaskController : Controller
    {
        public readonly MyDbConnect db;
       
        public TaskController(MyDbConnect _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
           
            return View();
        }
        //[RequirePermission(Permissions.User.List_Task)]
        //[RequirePermission(Permissions.Admin.List_Task)]
        public async Task<IActionResult> List_Task(int ? page)
        {
            try
            {
                KiemTra k = new KiemTra(db);
                List<string> DS = k.List(HttpContext);
                int page_number = 0;
                int page_size = 0;
                List<ItemTasks> list = new List<ItemTasks>();
                if (DS.Contains("Task.List_Task"))
                {
                    page_number = page ?? 1;
                    page_size = 5;
                    list = db.Tasks.OrderByDescending(s => s.CategoriesId).ToList();
                    ViewBag.errorSelectTask = TempData["errorSelectTask"];
                    ViewBag.create = TempData["createMessage"];
                    return View("List_Task", await list.ToPagedListAsync(page_number, page_size));
                }
                else
                {
                    string script = "<script>alert('Bạn không có đủ quyền, xin liên hệ với admin.');" +
                            "window.location.href = '" + Url.Action("Index", "Home") + "';</script>";
                    return Content(script, "text/html", System.Text.Encoding.UTF8);
                }
            }catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        public IActionResult Create()
        {
            KiemTra k = new KiemTra(db);
            List<string> DS = k.List(HttpContext);
            if (DS.Contains("Task.Create"))
            {
                ViewBag.task = "/Admin/Task/CreatePost";
                    return View("CreatePost");
            }
            else
            {
                string script = "<script>alert('Bạn không đủ quyền để thực hiện, Vui lòng liên hệ với Admin để truy cập!');" + "window.location='" + Url.Action("List_Task", "Task") + "';</script>";
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }
        }
        [HttpPost, ValidateAntiForgeryToken]
        //[RequirePermission(Permissions.Admin.CreatePost)]
        public IActionResult CreatePost(IFormCollection fc)
        {
            KiemTra k = new KiemTra(db);
            List<string> DS = k.List(HttpContext);
            if (DS.Contains("Task.Create"))
            {
                var decription = !String.IsNullOrEmpty(fc["Description"].ToString().Trim()) ? fc["Description"].ToString() : "";
                    var SoLuong = !string.IsNullOrEmpty(fc["SoLuong"]) ? Convert.ToInt32(fc["SoLuong"]) : 0;
                    var Title = !string.IsNullOrEmpty(fc["Title"].ToString().Trim()) ? fc["Title"].ToString() : "";
                    var CategoriesId = Convert.ToInt32(fc["CategoriesId"]);
                    int mark = 0;
                    if (fc.TryGetValue("congruation", out var congruationValue))
                    {
                        if (congruationValue.ToString().ToLower() == "on" || congruationValue.ToString().ToLower() == "true")
                        {
                            mark = 1;
                        }
                        else if (int.TryParse(congruationValue.ToString(), out int result))
                        {
                            mark = result;
                        }
                    }
                    var sanitizer = new Ganss.XSS.HtmlSanitizer();

                    // Lọc HTML để chỉ giữ lại các thẻ hợp lệ
                    //var safeHtml = sanitizer.Sanitize(decription);
                    //var safeTitle = sanitizer.Sanitize(Title);
                    try
                    {
                        ItemTasks cate = new ItemTasks();
                        cate.Decription = decription;
                        cate.Title = Title;
                        cate.SoLuong = SoLuong;
                        cate.Mark = mark;
                        cate.CategoriesId = CategoriesId;
                        cate.DateTime = DateTime.UtcNow;
                        db.Tasks.Add(cate);

                        db.SaveChanges();

                        TempData["createMessage"] = "Đã Tạo Mới Thành Công";
                    }
                    catch (Exception ex)
                    {
                        TempData["createMessage"] = "Đã Thất Bại Trong Quá Trình Tạo Mới: " + ex.Message;
                    }
                    return RedirectToAction("List_Task", "Task");
                }
            else
            {
                string script = "<script>alert('Bạn không đủ quyền để thực hiện, Vui lòng liiên hệ với Admin để truy cập!');" + "window.location='" + Url.Action("List_Task", "Task")+ "';</script>";
                return Content(script, "text/html");
            }            
        }
        //[RequirePermission(Permissions.Admin.Delete)]
        public IActionResult Delete(int ? id)
        {
            KiemTra k = new KiemTra(db);
            List<string> DS = k.List(HttpContext);
            if (DS.Contains("Task.Delete"))
            {
                    int taskId = id ?? 0;
                    ItemTasks task = db.Tasks.Where(s => s.TaskId == taskId).FirstOrDefault();
                    db.Tasks.Remove(task);
                    db.SaveChanges();
                    return RedirectToAction("List_Task", "Task");
            }
            else
            {
                string script = "<script>alert('Bạn không đủ quyền để thực hiện, Vui lòng liiên hệ với Admin để truy cập!');" + "window.location='" + Url.Action("List_Task", "Task") + "';</script>";
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }
        }
        
        public async Task<IActionResult> search(int? page)
        {
            KiemTra k = new KiemTra(db);
            List<string> DS = k.List(HttpContext);
            if (!DS.Contains("Task.search"))
            {
                string script = "<script>alert('Bạn không đủ quyền để thực hiện, Vui lòng liên hệ với Admin để truy cập!');" + "window.location='" + Url.Action("List_Task", "Task") + "';</script>";
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }

            IQueryable<ItemTasks> query = db.Tasks;

            if (Request.Query.TryGetValue("Searchkey", out var searchKeyValue) &&
                Request.Query.TryGetValue("select", out var selectValue))
            {
                string searchKey = searchKeyValue.ToString();
                string select = selectValue.ToString();

                switch (select)
                {
                    case "Title":
                        query = query.Where(s => s.Title.Contains(searchKey));
                        break;
                    case "Decription":
                        query = query.Where(s => s.Decription.Contains(searchKey));
                        break;
                    case "SoLuong":
                        if (int.TryParse(searchKey, out int sl))
                        {
                            query = query.Where(s => s.SoLuong < sl);
                        }
                        else
                        {
                            TempData["errorSelectTask"] = "Giá trị số lượng không hợp lệ";
                            return RedirectToAction("List_Task", "Task");
                        }
                        break;
                }

                ViewBag.Searchkey = searchKey;
                ViewBag.select = select;
            }

            query = query.OrderByDescending(s => s.TaskId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var suggestion = await query.Take(4).Select(t => new { t.Title, t.TaskId }).ToListAsync();
                return Json(suggestion);
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;

            var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);

            if (pagedList.Count == 0)
            {
                TempData["errorSelectTask"] = "Không có kết quả nào được tìm kiếm, Vui lòng thao tác lại";
                HttpContext.Session.SetString("task", TempData["errorSelectTask"].ToString());
            }

            ViewBag.errorSelectTask = TempData["errorSelectTask"];

            return View("search", pagedList);
        }
        public IActionResult Update(int ? id)
        {
            KiemTra k = new KiemTra(db);
            List<string> DS = k.List(HttpContext);
            if (DS.Contains("Task.Update"))
            {
                int _id = id ?? 0;
                ViewBag.task = "/Admin/Task/UpdatePost/" + _id;
                ViewBag.TaskId = id;
                var x = db.Tasks.Where(s => s.TaskId == _id).FirstOrDefault();

                return View("CreatePost", x);
            }
            else
            {
                string script = "<script>alert('Bạn không đủ quyền để thực hiện, Vui lòng liiên hệ với Admin để truy cập!');" + "window.location='" + Url.Action("List_Task", "Task") + "';</script>";
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }
        }
        //[RequirePermission(Permissions.Admin.UpdatePost)]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult UpdatePost(int id,IFormCollection fc)
        {
            KiemTra k = new KiemTra(db);
            List<string> DS = k.List(HttpContext);
            if (DS.Contains("Task.Update"))
            {
                var decription = !String.IsNullOrEmpty(fc["Description"].ToString().Trim()) ? fc["Description"].ToString() : "";
                var SoLuong = !string.IsNullOrEmpty(fc["SoLuong"]) ? Convert.ToInt32(fc["SoLuong"]) : 0;
                var Title = !string.IsNullOrEmpty(fc["Title"].ToString().Trim()) ? fc["Title"].ToString() : "";
                var CategoriesId = Convert.ToInt32(fc["CategoriesId"]);
                int mark = 0;
                if (fc.TryGetValue("congruation", out var congruationValue))
                {
                    if (congruationValue.ToString().ToLower() == "on" || congruationValue.ToString().ToLower() == "true")
                    {
                        mark = 1;
                    }
                    else if (int.TryParse(congruationValue.ToString(), out int result))
                    {
                        mark = result;
                    }
                }

                var x = db.Tasks.Where(s => s.TaskId == id).FirstOrDefault();
                if (x != null)
                {
                    x.Title = Title;
                    x.CategoriesId = CategoriesId;
                    x.Decription = decription;
                    x.Mark = mark;
                    x.SoLuong = SoLuong;
                    TempData["UpdateTask"] = "Cập nhật thành công";
                    db.SaveChanges();
                }
                else
                {
                    TempData["UpdateTask"] = "Cập nhật Task " + x.Title + " Thất bại, Vui lòng cập nhật lại!";

                }


                return RedirectToAction("List_Task", "Task");
            }
            else
            {
                string script = "<script>alert('Bạn không đủ quyền để thực hiện, Vui lòng liiên hệ với Admin để truy cập!');" + "window.location='" + Url.Action("List_Task", "Task") + "';</script>";
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }
        }

    }
}
