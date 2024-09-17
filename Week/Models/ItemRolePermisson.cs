using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Week.Models
{
    [Table("RolePermission")]
    public class ItemRolePermisson
    {
        [Key]
        public int RolePermissionId { get; set; }
        public int PermissionId { get; set; }
        public int RoleId { get; set; }
    }
}
