using System;
using System.Collections.Generic;

namespace prjErpDemo.Models;

public partial class Order
{
    public int OrderID { get; set; }

    public int CustomerID { get; set; }

    public DateTimeOffset OrderDate { get; set; }

    public int OrderStatusID { get; set; }

    public DateTimeOffset? ShippedDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual OrderStatus OrderStatus { get; set; } = null!;
}
