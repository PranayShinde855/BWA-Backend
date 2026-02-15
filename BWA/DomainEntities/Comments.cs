using System.ComponentModel.DataAnnotations.Schema;

namespace BWA.DomainEntities
{
    [Table("Comments")]
    public class Comments
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool IsLiked { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }

        [ForeignKey("BlogPostId")]
        public virtual BlogPost BlogPost { get; set; } = null!;
    }
}
