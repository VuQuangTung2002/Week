using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Week.Models
{
    public class ItemPosts
    {
        [Key]
        public int PostId { get; set; }
        public string TieuDe { get; set; }
        public string NoiDung { get; set; }
        public DateTime NgayDang { get; set; }
        public int Id { get; set; }
        public string Image { get; set; }
    }
}
