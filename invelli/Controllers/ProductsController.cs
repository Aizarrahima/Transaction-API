using invelli.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace invelli.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly Invelli_DB _invelli;
        public ProductsController(Invelli_DB invelli)
        {
            _invelli = invelli;
        }

        #region Get: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            if (_invelli.Products == null)
            {
                return NotFound();
            }

            return await _invelli.Products.ToListAsync();
        }
        #endregion

        #region Get: api/Products/{ID}
        [HttpGet("{ID}")]
        public async Task<ActionResult<Product>> GetProduct(Guid ID)
        {
            if (_invelli.Products == null) { return NotFound(); }

            var product = await _invelli.Products.FindAsync(ID);

            if (product == null)
            {
                return NotFound();
            }
            return product;
        }
        #endregion

        #region Post: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _invelli.Products.Add(product);
            await _invelli.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { ID = product.ID }, product);
        }
        #endregion

        #region Put: api/Products/{ID}
        [HttpPut("{ID}")]
        public async Task<IActionResult> UpdateProduct(Guid ID, Product product)
        {
            if (ID != product.ID)
            {
                return BadRequest();
            }

            _invelli.Entry(product).State = EntityState.Modified;
            try
            {
                await _invelli.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var updateProduct = await _invelli.Products.FindAsync(ID);

            return Ok(updateProduct);
        }

        private bool ProductExists(Guid ID)
        {
            return (_invelli.Products?.Any(product => product.ID == ID)).GetValueOrDefault();
        }
        #endregion

        #region Delete: api/Products/{ID}
        [HttpDelete("{ID}")]
        public async Task<IActionResult> ProductDelete(Guid ID)
        {
            if (_invelli.Products == null)
            {
                return NotFound();
            }

            var product = await _invelli.Products.FindAsync(ID);
            if (product == null)
            {
                return NotFound();
            }

            _invelli.Products.Remove(product);
            await _invelli.SaveChangesAsync();
            return Ok(new { message = "Delete Succes." });
        }
        #endregion
    }
}
