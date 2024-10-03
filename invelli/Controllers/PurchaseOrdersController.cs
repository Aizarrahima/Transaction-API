using invelli.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace invelli.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController: ControllerBase
    {
        private readonly Invelli_DB _invelli;
        public PurchaseOrdersController(Invelli_DB invelli)
        {
            _invelli = invelli;
        }

        #region Get: api/PurchaseOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetPurchaseOrders()
        {
            if(_invelli == null)
            {
                return NotFound();
            }

            return await _invelli.PurchaseOrders.Include(purchaseOrder => purchaseOrder.Supplier).ToListAsync(); // Eager Loading
        }
        #endregion

        #region Get:  api/PurchaseOrders/{ID}
        [HttpGet("{ID}")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrder(Guid ID)
        {
            if(_invelli.PurchaseOrders == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _invelli.PurchaseOrders
                .Include(purchaseOrder => purchaseOrder.Supplier) // Eager Loading
                .FirstOrDefaultAsync(purchaseOrder => purchaseOrder.ID == ID);

            if(purchaseOrder is null)
            {
                return NotFound();
            }
            return purchaseOrder;
        }
        #endregion

        #region Post: api/PurchaseOrders
        [HttpPost]
        public async Task<ActionResult<PurchaseOrder>> createPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            var existingSupplier = await _invelli.Suppliers.FindAsync(purchaseOrder.SupplierID);
            if (existingSupplier == null) {
                return BadRequest("Supplier does not exist.");
            }
            purchaseOrder.Supplier = existingSupplier;

            _invelli.PurchaseOrders.Add(purchaseOrder);
            await _invelli.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPurchaseOrders), new { ID = purchaseOrder.ID }, purchaseOrder);
        }
        #endregion

        #region Put: api/PurchaseOrders/{ID}
        [HttpPut("{ID}")]
        public async Task<IActionResult> PurchaseOrderUpdate(Guid ID, PurchaseOrder purchaseOrder)
        {
            if(ID != purchaseOrder.ID)
            {
                return BadRequest("Purchase Order Data Not Found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!SupplierExists(purchaseOrder.SupplierID))
            {
                return NotFound(new { Message = "Supplier Not Found." });
            }

            _invelli.Entry(purchaseOrder).State = EntityState.Modified;

            try
            {
                await _invelli.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderExists(ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var updatedPO = await _invelli.PurchaseOrders.Include(po => po.Supplier).FirstOrDefaultAsync(po => po.ID == ID);

            return Ok(updatedPO);
        }

        private bool SupplierExists(Guid SupplierID)
        {
            return (_invelli.Suppliers?.Any(s => s.ID == SupplierID)).GetValueOrDefault();
        }

        private bool PurchaseOrderExists(Guid ID)
        {
            return (_invelli.PurchaseOrders?.Any(po => po.ID == ID)).GetValueOrDefault();
        }
        #endregion

        #region Delete: api/PurchaseOrders/{ID}
        [HttpDelete("{ID}")]
        public async Task<ActionResult<PurchaseOrder>> PurchaseOrderDelete(Guid ID)
        {
            if (_invelli.PurchaseOrders is null)
            {
                return NotFound();
            }

            var purchaseOrder = await _invelli.PurchaseOrders.FindAsync(ID);
            
            if(purchaseOrder is null)
            {
                return NotFound();
            }

            _invelli.PurchaseOrders.Remove(purchaseOrder);
            await _invelli.SaveChangesAsync();
            return Ok(new { message = "Delete Succes." });
        }
        #endregion
    }
}
