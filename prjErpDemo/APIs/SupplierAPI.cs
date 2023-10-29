using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjErpDemo.Models;

namespace prjErpDemo.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierAPI : ControllerBase
    {
        private readonly db_ErpDemoContext _db;

        public SupplierAPI(db_ErpDemoContext db)
        {
            _db = db;
        }


        // DELETE: api/SupplierAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            Supplier? supplier = await _db.Suppliers.FirstOrDefaultAsync(s => s.SupplierID == id);

            if (supplier == null)
            {
                return NotFound(new { status = "Supplier not found" });
            }

            try
            {
                _db.Suppliers.Remove(supplier);
                await _db.SaveChangesAsync();
                return Ok(new { status = "Supplier deleted" });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                   ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    // Delete error caused by FK constraint
                    string messageRtn = "This supplier cannot be deleted due to existing product";
                    return StatusCode(500, new { status = "Delete failed", error = messageRtn });

                }
                else
                {
                    return StatusCode(500, new { status = "Delete failed", error = ex });
                }
            }
        }
    }
}

