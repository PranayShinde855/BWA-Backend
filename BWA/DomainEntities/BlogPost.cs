using System.ComponentModel.DataAnnotations.Schema;
using BWA.Utility;

namespace BWA.DomainEntities
{
    [Table("BlogPost")]
    public class BlogPost
    {
        public int Id { get; set; }
        public int CategoryId { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public byte[]? Image { get; set; }
        public string? ImageName { get; set; }
        public string? ImageExtension { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = Utils.CurrentDateTime;
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public int? DeletedBy { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User CreaedByUser { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public List<Comments> Comments { get; set; } = new List<Comments>();
    }
}
