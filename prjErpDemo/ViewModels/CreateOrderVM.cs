using prjErpDemo.Models;

namespace prjErpDemo.ViewModels
{
    public class CreateOrderVM
    {
        public CreateOrderVM()
        {
            customers = new List<Customer>();
            products = new List<Product>();
            productsSelected = new List<ProductSelected>();
        }

        public int customerID { get; set; }
        public List<Customer> customers { get; set; }
        public List<Product> products { get; set; }
        public List<ProductSelected> productsSelected { get; set; }
    }

    public class ProductSelected
    {
        public int productID { get; set; }
        public int quantityPurchased { get; set; }
    }
}
