using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjErpDemo.Models;

namespace prjErpDemo.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartAPI : ControllerBase
    {
        private readonly db_ErpDemoContext _db;

        public ChartAPI(db_ErpDemoContext db)
        {
            _db = db;
        }
        [HttpGet("MonthlySales")]
        public async Task<ActionResult> GetChart()
        {
            var currentDate = DateTime.Now;
            var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            try
            {
                var result = await _db.OrderDetails
                    .Include(od => od.Order)
                    .Include(od => od.Product)
                    .Where(od => od.Order.OrderDate >= firstDayOfMonth
                                && od.Order.OrderDate <= lastDayOfMonth
                                && od.Order.OrderStatusID == 3)
                    .GroupBy(od => od.Product.Category.CategoryName)
                    .Select(group => new
                    {
                        Category = group.Key,
                        TotalSale = group.Sum(od => od.Quantity * od.Product.Price)
                    })
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Failed to get data", error = ex });
            }
        }
    }
}
