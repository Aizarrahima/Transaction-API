using System.ComponentModel.DataAnnotations.Schema;

namespace invelli.Models
{
    public class Product
    {
        public Guid ID { get; set; }
        [Column(TypeName ="nvarchar(10)")]
        public string Code { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
    }
}
