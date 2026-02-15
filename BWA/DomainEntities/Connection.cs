using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BWA.DomainEntities
{
    [Table("Connection")]
    public class Connection
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; } = 0;
        public string JWTToken { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public bool IsLogout { get; set; }
    }
}
