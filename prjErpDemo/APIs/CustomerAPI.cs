using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjErpDemo.Models;

namespace prjErpDemo.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAPI : ControllerBase
    {
        private readonly db_ErpDemoContext _db;

        public CustomerAPI(db_ErpDemoContext db)
        {
            _db = db;
        }

        // GET: api/CustomerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
          if (_db.Customers == null)
          {
              return NotFound();
          }
            return await _db.Customers.ToListAsync();
        }

        // DELETE: api/CustomerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            Customer? customer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerID == id);

            if (customer == null)
            {
                return NotFound(new { status = "Customer not found" });
            }

            try
            {
                _db.Customers.Remove(customer);
                await _db.SaveChangesAsync();
                return Ok(new { status = "Customer deleted" });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                   ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    // Delete error caused by FK constraint
                    string messageRtn = "This customer cannot be deleted due to existing order";
                    return StatusCode(500, new { status = "Delete failed", error = messageRtn });

                }
                else
                {
                    return StatusCode(500, new { status = "Delete failed", error = ex });
                }
            }
        }

        private bool CustomerExists(int id)
        {
            return (_db.Customers?.Any(e => e.CustomerID == id)).GetValueOrDefault();
        }
    }
}
