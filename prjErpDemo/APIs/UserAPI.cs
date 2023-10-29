using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjErpDemo.Models;

namespace prjErpDemo.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPI : ControllerBase
    {
        private readonly db_ErpDemoContext _db;

        public UserAPI(db_ErpDemoContext db)
        {
            _db = db;
        }


        // DELETE: api/SupplierAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            User? user = await _db.Users.FirstOrDefaultAsync(u => u.UserID == id);

            if (user == null)
            {
                return NotFound(new { status = "User not found" });
            }

            try
            {
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                return Ok(new { status = "User deleted" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Delete failed", error = ex });
            }
        }
    }
}

