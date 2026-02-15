using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BWA.DomainEntities
{
    [Table("Country")]
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string IsdCode { get; set; }
    }
}
