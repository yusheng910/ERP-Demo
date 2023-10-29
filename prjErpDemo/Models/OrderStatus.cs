using System;
using System.Collections.Generic;

namespace prjErpDemo.Models;

public partial class OrderStatus
{
    public int OrderStatusID { get; set; }

    public string StatusDescription { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
