using Microsoft.EntityFrameworkCore;

namespace invelli.Models
{
    public class Invelli_DB: DbContext
    {
        public Invelli_DB(DbContextOptions<Invelli_DB> options) : base(options) 
        {
            // Enable Lazy Loading
            this.ChangeTracker.LazyLoadingEnabled = true;
        }

        public DbSet<Supplier> Suppliers { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<PurchaseOrder> PurchaseOrders { get; set;} = null!;
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = null!;
    }
}
