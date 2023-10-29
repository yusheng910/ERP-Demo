using Microsoft.AspNetCore.Mvc;
using prjErpDemo.Filters;
using prjErpDemo.Models;

namespace prjErpDemo.Controllers
{
    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    public class CustomerController : Controller
    {
        private readonly db_ErpDemoContext _db;
        public CustomerController(db_ErpDemoContext db)
        {
            _db = db;
        }
        public IActionResult CustomerList()
        {
            IEnumerable<Customer> customers = from cus in _db.Customers
                                              select cus;

            return View(customers);
        }

        public IActionResult CreateCustomer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCustomer(Customer customerParam)
        {
            if (String.IsNullOrEmpty(customerParam.CustomerName) ||
                String.IsNullOrEmpty(customerParam.Email) ||
                String.IsNullOrEmpty(customerParam.Country) ||
                String.IsNullOrEmpty(customerParam.Address))
            {
                return BadRequest("Please provide correct information");
            }
            Customer? customerCheck = _db.Customers.FirstOrDefault(c => c.CustomerName == customerParam.CustomerName);

            if (customerCheck != null)
            {
                return BadRequest("This customer is already in the list.");
            }

            try
            {
                _db.Add(customerParam);
                _db.SaveChanges();
                return RedirectToAction("CustomerList", "Customer");
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong with the database: " + ex);
            }
        }

        // update customer
        public IActionResult UpdateCustomer(int id)
        {
            Customer? customerToUpdate = _db.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customerToUpdate != null)
            {
                return View(customerToUpdate);
            }

            return RedirectToAction("ProductList", "Product");
        }

        [HttpPost]
        public IActionResult UpdateCustomer(Customer customerParam)
        {
            Customer? customer = _db.Customers.FirstOrDefault(c => c.CustomerID == customerParam.CustomerID);
            if (customer != null)
            {
                bool customerNameConflict = _db.Customers.Any(c => c.CustomerName == customerParam.CustomerName && c.CustomerID != customerParam.CustomerID);
                if (String.IsNullOrEmpty(customerParam.CustomerName) ||
                    String.IsNullOrEmpty(customerParam.Email) ||
                    String.IsNullOrEmpty(customerParam.Country) ||
                    String.IsNullOrEmpty(customerParam.Address))
                {
                    return BadRequest("Please provide correct information");
                }

                if (!customerNameConflict)
                {
                    try
                    {
                        customer.CustomerName = customerParam.CustomerName;
                        customer.Email = customerParam.Email;
                        customer.Phone = customerParam.Phone;
                        customer.Country = customerParam.Country;
                        customer.Address = customerParam.Address;
                        _db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(("Something went wrong with the database: " + ex));
                    }
                }
                else
                {
                    return BadRequest("Customer name conflicts with an existing customer.");
                }
            }
            return RedirectToAction("CustomerList", "Customer");
        }
    }
}
