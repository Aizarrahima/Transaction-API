using invelli.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace invelli.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderDetailController : ControllerBase
    {
        private readonly Invelli_DB _invelli;

        public PurchaseOrderDetailController(Invelli_DB invelli)
        {
            _invelli = invelli;
        }

        #region Get: api/PurchaseOrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrderDetail>>> GetPurchaseOrderDetails()
        {
            if (_invelli == null)
            {
                return NotFound();
            }

            return await _invelli.PurchaseOrderDetails
                    .Include(pod => pod.PurchaseOrder)  // Eager load PurchaseOrder
                    .Include(pod => pod.Product)        // Eager load Product
                    .ToListAsync();
        }
        #endregion

        #region Get: api/PurchaseOrderDetails/{ID}
        [HttpGet("{ID}")]
        public async Task<ActionResult<PurchaseOrderDetail>> GetPurchaseOrderDetail(Guid ID)
        {
            if (_invelli.PurchaseOrderDetails == null)
            {
                return NotFound();
            }

            var purchaseOrderDetail = await _invelli.PurchaseOrderDetails
                .Include(pod => pod.PurchaseOrder) // Eager Loading
                .Include(pod => pod.Product)
                .FirstOrDefaultAsync(purchaseOrder => purchaseOrder.ID == ID);

            if (purchaseOrderDetail is null)
            {
                return NotFound();
            }
            return purchaseOrderDetail;
        }
        #endregion

        #region Post: api/PurchaseOrderDetails
        [HttpPost]
        public async Task<ActionResult<PurchaseOrderDetail>> CreatePODetail(PurchaseOrderDetail purchaseOrderDetail)
        {
            var existingPO = await _invelli.PurchaseOrders.FindAsync(purchaseOrderDetail.PurchaseOrderID);
            if(existingPO == null)
            {
                return BadRequest("Purchase Order Data does not exist.");
            }
            purchaseOrderDetail.PurchaseOrder = existingPO;

            var existingProduct = await _invelli.Products.FindAsync(purchaseOrderDetail.ProductID);
            if(existingProduct == null)
            {
                return BadRequest("Product does not exist.");
            }
            purchaseOrderDetail.Product = existingProduct;

            _invelli.PurchaseOrderDetails.Add(purchaseOrderDetail);
            await _invelli.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPurchaseOrderDetail), new { ID = purchaseOrderDetail.ID }, purchaseOrderDetail);

        }
        #endregion

        #region Put: api/PurchaseOrderDetails/{ID}
        [HttpPut("{ID}")]
        public async Task<IActionResult> PurchaseOrderDetailUpdate(Guid ID, PurchaseOrderDetail purchaseOrderDetail)
        {
            if (ID != purchaseOrderDetail.ID)
            {
                return BadRequest("Detail Purchase Order Not Found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!PurchaseOrderExists(purchaseOrderDetail.PurchaseOrderID))
            {
                return NotFound(new { Message = "Purchase Order Not Found." });
            }

            if (!ProductExists(purchaseOrderDetail.ProductID))
            {
                return NotFound(new { Message = "Product Not Found." });
            }

            _invelli.Entry(purchaseOrderDetail).State = EntityState.Modified;

            try
            {
                await _invelli.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderDetailExists(ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var updatedPODetail = await _invelli.PurchaseOrderDetails
                .Include(p => p.Product)
                .Include(po => po.PurchaseOrder)
                .FirstOrDefaultAsync(po => po.ID == ID);

            return Ok(updatedPODetail);
        }

        private bool ProductExists(Guid ProductID)
        {
            return (_invelli.Products?.Any(p => p.ID == ProductID)).GetValueOrDefault();
        }

        private bool PurchaseOrderExists(Guid PurchaseOrderID)
        {
            return (_invelli.PurchaseOrders?.Any(po => po.ID == PurchaseOrderID)).GetValueOrDefault();
        }

        private bool PurchaseOrderDetailExists(Guid ID)
        {
            return (_invelli.PurchaseOrderDetails?.Any(pod => pod.ID == ID)).GetValueOrDefault();
        }
        #endregion

        #region Delete: api/PurchaseOrderDetails/{ID}
        [HttpDelete("{ID}")]
        public async Task<ActionResult<PurchaseOrderDetail>> DeleteDetail(Guid ID)
        {
            if(_invelli.PurchaseOrderDetails is null)
            {
                return NotFound();
            }

            var PODetail = await _invelli.PurchaseOrderDetails.FindAsync(ID);
            if(PODetail is null)
            {
                return NotFound();
            }

            _invelli.PurchaseOrderDetails.Remove(PODetail);
            await _invelli.SaveChangesAsync();
            return Ok(new { message = "Delete Success." });
        }
        #endregion
    }
}
