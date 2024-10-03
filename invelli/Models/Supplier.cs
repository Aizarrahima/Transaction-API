using System.ComponentModel.DataAnnotations.Schema;

namespace invelli.Models
{
    public class Supplier
    {
        public Guid ID { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string Code { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string City { get; set; }
    }
}
