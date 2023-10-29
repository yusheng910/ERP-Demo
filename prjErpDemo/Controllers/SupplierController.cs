using Microsoft.AspNetCore.Mvc;
using prjErpDemo.Filters;
using prjErpDemo.Models;

namespace prjErpDemo.Controllers
{
    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    public class SupplierController : Controller
    {
        private readonly db_ErpDemoContext _db;
        public SupplierController(db_ErpDemoContext db)
        {
            _db = db;
        }

        public IActionResult SupplierList()
        {
            IEnumerable<Supplier> suppliers = from sup in _db.Suppliers
                                              select sup;

            return View(suppliers);
        }

        public IActionResult CreateSupplier()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateSupplier(Supplier supplierParam)
        {
            if (String.IsNullOrEmpty(supplierParam.SupplierName) ||
                String.IsNullOrEmpty(supplierParam.Email))
            {
                return BadRequest("Please provide correct information");
            }
            Supplier? supplierCheck = _db.Suppliers.FirstOrDefault(s => s.SupplierName == supplierParam.SupplierName);

            if (supplierCheck != null)
            {
                return BadRequest("This supplier is already in the list.");
            }

            try
            {
                _db.Add(supplierParam);
                _db.SaveChanges();
                return RedirectToAction("SupplierList", "Supplier");
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong with the database: " + ex);
            }
        }

        // update Supplier
        public IActionResult UpdateSupplier(int id)
        {
            Supplier? supplierToUpdate = _db.Suppliers.FirstOrDefault(s => s.SupplierID == id);
            if (supplierToUpdate != null)
            {
                return View(supplierToUpdate);
            }

            return RedirectToAction("SupplierList", "Supplier");
        }

        [HttpPost]
        public IActionResult UpdateSupplier(Supplier supplierParam)
        {
            Supplier? supplier = _db.Suppliers.FirstOrDefault(s => s.SupplierID == supplierParam.SupplierID);
            if (supplier != null)
            {
                bool supplierNameConflict = _db.Suppliers.Any(s => s.SupplierName == supplierParam.SupplierName && s.SupplierID != supplierParam.SupplierID);
                if (String.IsNullOrEmpty(supplierParam.SupplierName) ||
                    String.IsNullOrEmpty(supplierParam.Email))
                {
                    return BadRequest("Please provide correct information");
                }

                if (!supplierNameConflict)
                {
                    try
                    {
                        supplier.SupplierName = supplierParam.SupplierName;
                        supplier.ContactName = supplierParam.ContactName;
                        supplier.Email = supplierParam.Email;
                        supplier.Phone = supplierParam.Phone;
                        _db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(("Something went wrong with the database: " + ex));
                    }
                }
                else
                {
                    return BadRequest("Supplier name conflicts with an existing supplier.");
                }
            }
            return RedirectToAction("SupplierList", "Supplier");
        }
    }
}
