using Week.Areas.Admin.Attributes;
using Week.Models;

namespace Week.Areas.Admin.Attributes
{
    public class KiemTra
    {
        // liet ke cacs quyen cua du lieu ra 
        public readonly MyDbConnect db;
        public KiemTra(MyDbConnect dbcontext)
        {
            db = dbcontext;
        }
        public List<string> List(HttpContext x)
        {
            // Tìm email của người dùng từ session
            var user_email = x.Session.GetString("user_email")?.Trim();
            if (string.IsNullOrEmpty(user_email))
            {
                return new List<string> { "No email found in session" };
            }

            // Tìm người dùng với email
            ItemUsers email = db.Users.FirstOrDefault(s => s.Email == user_email);
            if (email == null)
            {
                return new List<string> { "User not found" };
            }
            int id = email.Id;

           

            // Lấy danh sách các quyền của người dùng
            var roles = db.Roles.Where(r => r.Id == id).ToList();

            if (!roles.Any())
            {
                return new List<string> { "No roles assigned to this user" };
            }
            var permissions = (from role in roles
                               join rolePermission in db.RolePermisson on role.RoleId equals rolePermission.RoleId
                               join permission in db.Permisson on rolePermission.PermissionId equals permission.PermissionId
                               select permission.NamePermission
                               ).Distinct().ToList();

            // Trả về danh sách quyền dưới dạng chuỗi (các quyền được nối bằng dấu phẩy)
            return permissions;
        }
    }
}
