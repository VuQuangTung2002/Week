using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Week.Models
{
    [Table("Role")]
    public class ItemRole : IdentityRole<int>
    {
        [Key]
        public int RoleId { get; set; }
        [Required]
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public int Id{ get; set; }

        // theo cách sử dụng entityfameword core theo cơ chế query
        }
}
