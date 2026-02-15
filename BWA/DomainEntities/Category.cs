using System.ComponentModel.DataAnnotations.Schema;

namespace BWA.DomainEntities
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
