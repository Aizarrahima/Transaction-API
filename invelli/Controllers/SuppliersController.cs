using invelli.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace invelli.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly Invelli_DB _invelli;
        public SuppliersController(Invelli_DB invelli)
        {
            _invelli = invelli;
        }

        #region Get: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            if(_invelli.Suppliers == null)
            {
                return NotFound();
            }

            return await _invelli.Suppliers.ToListAsync();
        }
        #endregion

        #region Get: api/Suppliers/{ID}
        [HttpGet("{ID}")]
        public async Task<ActionResult<Supplier>> GetSupplier(Guid ID)
        {
            if (_invelli.Suppliers == null) { return NotFound(); }

            var supplier = await _invelli.Suppliers.FindAsync(ID);

            if (supplier == null)
            {
                return NotFound();
            }
            return supplier;
        }
        #endregion

        #region Post: api/Suppliers
        [HttpPost]
        public async Task<ActionResult<Supplier>> CreateSupplier(Supplier supplier)
        {
            _invelli.Suppliers.Add(supplier);
            await _invelli.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSupplier), new { ID = supplier.ID }, supplier);
        }
        #endregion

        #region Put: api/Suppliers/{ID}
        [HttpPut("{ID}")]
        public async Task<IActionResult> UpdateSupplier(Guid ID, Supplier supplier)
        {
            if(ID != supplier.ID)
            {
                return BadRequest();
            }

            _invelli.Entry(supplier).State = EntityState.Modified;
            try
            {
                await _invelli.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var updateSupplier = await _invelli.Suppliers.FindAsync(ID);

            return Ok(updateSupplier);
        }

        private bool SupplierExists(Guid ID)
        {
            return (_invelli.Suppliers?.Any(supplier => supplier.ID == ID)).GetValueOrDefault();
        }
        #endregion

        #region Delete: api/Suppliers/{ID}
        [HttpDelete("{ID}")]
        public async Task<IActionResult> SupplierDelete(Guid ID)
        {
            if(_invelli.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _invelli.Suppliers.FindAsync(ID);
            if(supplier == null)
            {
                return NotFound();
            }

            _invelli.Suppliers.Remove(supplier);
            await _invelli.SaveChangesAsync();
            return Ok(new { message = "Delete Succes." });
        }
        #endregion
    }
}
