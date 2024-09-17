using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Week.Models
{
    [Table("Permission")]
    public class ItemPermission
    {
        [Key]
        public int PermissionId { get; set; }
        public string NamePermission { get; set; }

    }
}
