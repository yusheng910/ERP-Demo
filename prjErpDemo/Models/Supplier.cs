using System;
using System.Collections.Generic;

namespace prjErpDemo.Models;

public partial class Supplier
{
    public int SupplierID { get; set; }

    public string SupplierName { get; set; } = null!;

    public string? ContactName { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
