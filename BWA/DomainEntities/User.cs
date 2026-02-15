using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BWA.DomainEntities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public byte[]? Image { get; set; } = null;
        public string? ImageName { get; set; }
        public string? ImageExtension { get; set; }
        public string Salutation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ISDCode { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeactivatedDate { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }
    }
}
