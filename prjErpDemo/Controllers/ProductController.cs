using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjErpDemo.Filters;
using prjErpDemo.Models;
using prjErpDemo.ViewModels;

namespace prjErpDemo.Controllers
{
    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    public class ProductController : Controller
    {
        private readonly db_ErpDemoContext _db;
        public ProductController(db_ErpDemoContext db)
        {
            _db = db;
        }
        public IActionResult ProductList()
        {
            IEnumerable<Product>? products = null;
            products = _db.Products.Include(p => p.Category).Include(p => p.Supplier);


            List<ProductVM> productList = new List<ProductVM>();
            foreach (Product pro in products)
            {
                productList.Add(new ProductVM() { product = pro });
            }
            return View(productList);
        }

        public IActionResult ProductList2()
        {
            IEnumerable<Product>? products = null;
            products = _db.Products.Include(p => p.Category).Include(p => p.Supplier);

            return View(products);
        }

        public IActionResult ProductList3()
        {
            return View();
        }

        // create product
        public IActionResult CreateProduct()
        {
            var categories = _db.Categories.ToList();
            var suppliers = _db.Suppliers.ToList();

            ViewBag.Category = new SelectList(categories, "CategoryID", "CategoryName");
            ViewBag.Supplier = new SelectList(suppliers, "SupplierID", "SupplierName");
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(Product productParam)
        {
            if (String.IsNullOrEmpty(productParam.ProductName) ||
                productParam.Price <= 0 ||
                productParam.StockQuantity <= 0 ||
                productParam.CategoryID == 0 ||
                productParam.SupplierID == 0)
            {
                return BadRequest("Please provide correct information");
            }
            Product? productCheck = _db.Products.FirstOrDefault(p => p.ProductName == productParam.ProductName);

            if (productCheck != null)
            {
                return BadRequest("This product is already in the list.");
            }

            try
            {
                _db.Add(productParam);
                _db.SaveChanges();
                return RedirectToAction("ProductList", "Product");
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong with the database: " + ex);
            }
        }

        // update product
        public IActionResult UpdateProduct(int id)
        {
            var categories = _db.Categories.ToList();
            var suppliers = _db.Suppliers.ToList();

            ViewBag.Category = new SelectList(categories, "CategoryID", "CategoryName");
            ViewBag.Supplier = new SelectList(suppliers, "SupplierID", "SupplierName");

            Product? productToUpdate = _db.Products.FirstOrDefault(pro => pro.ProductID == id);
            if (productToUpdate != null)
            {
                return View(new ProductVM() { product = productToUpdate });
            }

            return RedirectToAction("ProductList", "Product");
        }

        [HttpPost]
        public IActionResult UpdateProduct(ProductVM productParam)
        {
            Product? product = _db.Products.FirstOrDefault(p => p.ProductID == productParam.productID);
            if (product != null)
            {
                bool productNameConflict = _db.Products.Any(p => p.ProductName == productParam.productName && p.ProductID != productParam.productID);
                if (String.IsNullOrEmpty(productParam.productName) ||
                    productParam.price <= 0 ||
                    productParam.stockQuantity <= 0 ||
                    productParam.categoryID == 0 ||
                    productParam.supplierID == 0)
                { 
                    return BadRequest("Please provide correct information"); 
                }

                if (!productNameConflict)
                {
                    try
                    {
                        product.ProductName = productParam.productName;
                        product.Price = productParam.price;
                        product.StockQuantity = productParam.stockQuantity;
                        product.CategoryID = productParam.categoryID;
                        product.SupplierID = productParam.supplierID;
                        _db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(("Something went wrong with the database: " + ex));
                    }
                }
                else
                {
                    return BadRequest("Product name conflicts with an existing product.");
                }
            }
            return RedirectToAction("ProductList", "Product");
        }
    }
}
