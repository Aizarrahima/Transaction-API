using System.ComponentModel.DataAnnotations.Schema;

namespace invelli.Models
{
    public class PurchaseOrderDetail
    {
        public Guid ID { get; set; }
        public Guid PurchaseOrderID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        public virtual Product Product { get; set; } // Lazy Loading
        public virtual PurchaseOrder PurchaseOrder { get; set; } // Lazy Loading
    }
}
