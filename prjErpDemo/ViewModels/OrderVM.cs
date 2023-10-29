using prjErpDemo.Models;

namespace prjErpDemo.ViewModels
{
    public class OrderVM
    {
        public OrderVM() { order = new Order(); orderDate = string.Empty; shippedDate = string.Empty; }
        public Order order { get; set; }
        public int orderID
        {
            get { return this.order.OrderID; }
            set { this.order.OrderID = value; }
        }

        public int customerID
        {
            get { return this.order.CustomerID; }
            set { this.order.CustomerID = value; }
        }

        public int orderStatusID
        {
            get { return this.order.OrderStatusID; }
            set { this.order.OrderStatusID = value; }
        }

        public string orderDate
        {
            get;
            set;
        }

        public string? shippedDate
        {
            get;
            set;
        }

        public string? customerName
        {
            get
            {
                if (this.order.Customer != null)
                {
                    return this.order.Customer.CustomerName;
                }
                return null;
            }
        }

        public string? orderStatus
        {
            get
            {
                if (this.order.OrderStatus != null)
                {
                    return this.order.OrderStatus.StatusDescription;
                }
                return null;
            }
        }

        public decimal totalAmount { get; set; }
        public List<OrderDetail> orderDetails { get; set; } = new List<OrderDetail>();
    }
}
