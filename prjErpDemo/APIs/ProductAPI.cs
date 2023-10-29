using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjErpDemo.Models;

namespace prjErpDemo.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPI : ControllerBase
    {
        private readonly db_ErpDemoContext _db;

        public ProductAPI(db_ErpDemoContext db)
        {
            _db = db;
        }

        // GET: api/ProductAPI
        [HttpGet]
        public async Task<ActionResult> GetProducts(string? keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                var products = await _db.Products
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .ToListAsync();

                var result = products.Select(p => new
                {
                    productID = p.ProductID,
                    productName = p.ProductName,
                    price = p.Price.ToString(), // toString to keep the decimal value ,
                    stock = p.StockQuantity,
                    category = p.Category.CategoryName,
                    supplier = p.Supplier.SupplierName
                });

                return Ok(result);
            }
            else
            {
                var products = await _db.Products
                    .Where(pro => pro.ProductName.Contains(keyword) ||
                                  pro.Supplier.SupplierName.Contains(keyword) ||
                                  pro.Category.CategoryName.Contains(keyword))
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    .ToListAsync();

                var result = products.Select(p => new
                {
                    productID = p.ProductID,
                    productName = p.ProductName,
                    price = p.Price.ToString(), // toString to keep the decimal value 
                    stock = p.StockQuantity,
                    category = p.Category.CategoryName,
                    supplier = p.Supplier.SupplierName
                });

                return Ok(result);
            }
        }

        // GET: api/ProductAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (_db.Products == null)
            {
                return NotFound();
            }
            var product = await _db.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/ProductAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest();
            }

            _db.Entry(product).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (_db.Products == null)
            {
                return Problem("Entity set 'db_ErpDemoContext.Products'  is null.");
            }
            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductID }, product);
        }

        // DELETE: api/ProductAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product? product = await _db.Products.FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
            {
                return NotFound(new { status = "Product not found" });
            }

            try
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return Ok(new { status = "Product deleted" });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    // Delete error caused by FK constraint
                    string messageRtn = "This product cannot be deleted due to existing order";
                    return StatusCode(500, new { status = "Delete failed", error = messageRtn });

                }
                else
                {
                    return StatusCode(500, new { status = "Delete failed", error = ex });
                }
            }
        }


        private bool ProductExists(int id)
        {
            return (_db.Products?.Any(e => e.ProductID == id)).GetValueOrDefault();
        }
    }
}
