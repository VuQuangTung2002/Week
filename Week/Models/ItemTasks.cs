using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Week.Models
{
    [Table("Task")]
    public class ItemTasks
    {
        [Key]
        public int TaskId { get; set; }
        
        public string Title { get; set; }
        public string Decription { get; set; }
        public int SoLuong { get; set; }
        public int Mark { get; set; }
        public int CategoriesId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
