using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Week.Models
{
    [Table("UserToken")]
    public class ItemUserToken
    {
        [Key]
        public int UserTokenId { get; set; }
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpriredDateAccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpriredDateRefreshToken { get; set; }
        public string CodeRefreshToken {  get; set; }
        public int IsActive { get; set; }
    }
}
