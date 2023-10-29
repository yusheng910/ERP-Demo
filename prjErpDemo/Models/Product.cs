using System;
using System.Collections.Generic;

namespace prjErpDemo.Models;

public partial class Product
{
    public int ProductID { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public int CategoryID { get; set; }

    public int SupplierID { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Supplier Supplier { get; set; } = null!;
}
