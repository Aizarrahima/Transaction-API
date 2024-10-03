using System.ComponentModel.DataAnnotations.Schema;

namespace invelli.Models
{
    public class PurchaseOrder
    {
        public Guid ID { get; set; }
        [Column(TypeName = "nvarchar(10)")]
        public string Code { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid SupplierID { get; set; }
        [Column(TypeName = "nvarchar(Max)")]
        public string Remarks { get; set; }
        public virtual Supplier Supplier { get; set; } // Lazy Loading
    }
}
