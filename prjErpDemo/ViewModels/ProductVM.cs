using prjErpDemo.Models;

namespace prjErpDemo.ViewModels
{
    public class ProductVM
    {
        public ProductVM() { product = new Product(); }
        public Product product { get; set; }
        public int productID
        {
            get { return this.product.ProductID; }
            set { this.product.ProductID = value;}
        }

        public string productName
        {
            get { return this.product.ProductName; }
            set { this.product.ProductName = value; }
        }

        public decimal price
        {
            get { return this.product.Price; }
            set { this.product.Price = value; }
        }

        public int stockQuantity
        {
            get { return this.product.StockQuantity; }
            set { this.product.StockQuantity = value; }
        }

        public int categoryID
        {
            get { return this.product.CategoryID; }
            set { this.product.CategoryID = value; }
        }

        public int supplierID
        {
            get { return this.product.SupplierID; }
            set { this.product.SupplierID = value; }
        }

        public string? categoryName
        {
            get
            {
                if (this.product.Category != null)
                { 
                    return this.product.Category.CategoryName; 
                }
                return null;
            }
        }

        public string? supplierName
        {
            get
            {
                if (this.product.Supplier != null)
                {
                    return this.product.Supplier.SupplierName;
                }
                return null;
            }

        }
    }
}
