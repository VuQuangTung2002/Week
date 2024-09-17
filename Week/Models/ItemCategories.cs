using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Week.Models
{
    [Table("Categories")]
    public class ItemCategories
    {
        [Key]
        public int CategoriesId {  get; set; }
        public string CategoriesName {  get; set;}
        public int SoLuong {get; set;}
    }
}
