using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjErpDemo.Filters;
using prjErpDemo.Models;
using prjErpDemo.ViewModels;

namespace prjErpDemo.Controllers
{
    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    public class OrderController : Controller
    {
        private readonly db_ErpDemoContext _db;
        public OrderController(db_ErpDemoContext db)
        {
            _db = db;
        }
        public IActionResult OrderList()
        {
            // Query approach 1
            // Take too much time on querying data
            /*
            IEnumerable<Order> orders = _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetails)
                .OrderBy(o => o.OrderDate)
                .ToList();

            List<OrderVM> orderList = new List<OrderVM>();
            foreach (var o in orders)
            {
                string formattedOrderDateString = o.OrderDate.ToString("yyyy-MM-dd HH:mm:ssZ");
                string? formattedShippedDateString = o.ShippedDate?.ToString("yyyy-MM-dd HH:mm:ssZ");

                decimal totalAmount = 0;
                // calculating total amount for every order
                foreach (var orderDetail in o.OrderDetails)
                {
                    decimal productPrice = _db.Products
                        .Where(p => p.ProductID == orderDetail.ProductID)
                        .Select(p => p.Price)
                        .FirstOrDefault();

                    // get the amount of every detail and add to total amount 
                    decimal orderDetailTotal = productPrice * orderDetail.Quantity;
                    totalAmount += orderDetailTotal;
                }

                orderList.Add(new OrderVM
                {
                    order = o,
                    orderDate = formattedOrderDateString,
                    shippedDate = formattedShippedDateString,
                    totalAmount = totalAmount,
                });
            }
            return View(orderList);
            */

            // Query approach 2
            // improve the query effiency significantly
            List<OrderVM> orderList = _db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetails)
                .Select(o => new OrderVM
                {
                    order = o,
                    orderDate = o.OrderDate.ToString("yyyy-MM-dd HH:mm:ssZ"),
                    shippedDate = o.ShippedDate != null ? o.ShippedDate.Value.ToString("yyyy-MM-dd HH:mm:ssZ") : null,
                    totalAmount = o.OrderDetails.Sum(od => od.Product.Price * od.Quantity)
                })
                .OrderByDescending(o => o.order.OrderDate)
                .ToList();

            return View(orderList);
        }

        // create order
        public IActionResult CreateOrder()
        {
            var customerList = _db.Customers.ToList();
            var productList = _db.Products.ToList();

            var vm = new CreateOrderVM
            {
                customers = customerList,
                products = productList
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult CreateOrder(CreateOrderVM OrderParam)
        {
            if (OrderParam.productsSelected.Count == 0)
            {
                return BadRequest("No products have been added to the order");
            }

            try
            {
                // create new order
                Order newOrder = new Order
                {
                    CustomerID = OrderParam.customerID,
                    OrderDate = DateTimeOffset.UtcNow,
                    OrderStatusID = 1, // set default status == 1 (Pending)
                };
                _db.Add(newOrder);
                _db.SaveChanges(); // save & create orderID

                // create ordeDetail and adjust product stock accordingly
                foreach (var productSelected in OrderParam.productsSelected)
                {
                    Product? product = _db.Products.FirstOrDefault(p => p.ProductID == productSelected.productID);
                    if (product != null)
                    {
                        if (product.StockQuantity >= productSelected.quantityPurchased)
                        {
                            product.StockQuantity -= productSelected.quantityPurchased;
                            _db.Add(new OrderDetail
                            {
                                OrderID = newOrder.OrderID,
                                ProductID = productSelected.productID,
                                Quantity = productSelected.quantityPurchased
                            });
                        }
                        else
                        {
                            // not enough stock condition (should not happen with front-end JS check)                               
                            return BadRequest("Insufficient stock for some products");
                        }
                    }
                }

                _db.SaveChanges();
                return RedirectToAction("OrderList", "Order");

            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong with the database: " + ex.Message);
            }
        }

        // update order
        public IActionResult UpdateOrder(int id)
        {
            var orderStatus = _db.OrderStatuses.ToList();
            ViewBag.Status = new SelectList(orderStatus, "OrderStatusID", "StatusDescription");

            Order? orderToUpdate = _db.Orders
                .Include(ord => ord.Customer)
                .Include(ord => ord.OrderStatus)
                .Include(ord => ord.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefault(ord => ord.OrderID == id);

            if (orderToUpdate != null)
            {
                string formattedOrderDateString = orderToUpdate.OrderDate.ToString("yyyy-MM-dd HH:mm:ssZ");
                string? formattedShippedDateString = orderToUpdate.ShippedDate?.ToString("yyyy-MM-dd HH:mm:ssZ");

                List<OrderDetail> orderDetails = _db.OrderDetails
                    .Include(od => od.Product)
                    .Where(od => od.OrderID == orderToUpdate.OrderID)
                    .ToList();

                decimal totalAmount = 0;
                // calculating total amount for every order
                foreach (var orderDetail in orderDetails)
                {
                    // get the amount of every detail and add to total amount 
                    decimal productPrice = orderDetail.Product.Price;
                    decimal orderDetailTotal = productPrice * orderDetail.Quantity;
                    totalAmount += orderDetailTotal;
                }

                return View(new OrderVM()
                {
                    order = orderToUpdate,
                    orderDate = formattedOrderDateString,
                    shippedDate = formattedShippedDateString,
                    totalAmount = totalAmount,
                    orderDetails = orderDetails
                });
            }

            return RedirectToAction("OrderList", "Order");
        }

        [HttpPost]
        public IActionResult UpdateOrder(OrderVM orderParam)
        {
            Order? order = _db.Orders.FirstOrDefault(o => o.OrderID == orderParam.orderID);
            if (order != null)
            {
                try
                {
                    // check if the requested status change is valid based on the current status
                    // if the current status is "Pending" (1)
                    if (order.OrderStatusID == 1)
                    {
                        if (orderParam.orderStatusID == 1 || orderParam.orderStatusID == 2)
                        {
                            // set shippedDate for "Shipped" status
                            if (orderParam.orderStatusID == 2)
                            {
                                order.ShippedDate = DateTimeOffset.UtcNow;
                                order.OrderStatusID = orderParam.orderStatusID;
                            }
                        }
                        else if (orderParam.orderStatusID == 4)
                        {
                            List<OrderDetail> orderDetails = _db.OrderDetails.Where(od => od.OrderID == order.OrderID).ToList();
                            foreach (OrderDetail orderDetail in orderDetails)
                            {
                                Product? product = _db.Products.FirstOrDefault(p => p.ProductID == orderDetail.ProductID);
                                if (product != null)
                                {
                                    product.StockQuantity += orderDetail.Quantity;
                                }
                            }
                            order.OrderStatusID = orderParam.orderStatusID;
                        }
                        else
                        {
                            return BadRequest("Order should be shipped before completion.");
                        }
                    }
                    // if the current status is "Shipped" (2)
                    else if (order.OrderStatusID == 2)
                    {
                        if (orderParam.orderStatusID == 1)
                        {
                            // set shippedDate to null for "pending" (1)
                            order.ShippedDate = null;
                        }
                        else if (orderParam.orderStatusID == 4)
                        {
                            // set shippedDate to null for "canceled" (4) and put quantity back to stock
                            order.ShippedDate = null;
                            List<OrderDetail> orderDetails = _db.OrderDetails.Where(od => od.OrderID == order.OrderID).ToList();
                            foreach (OrderDetail orderDetail in orderDetails)
                            {
                                Product? product = _db.Products.FirstOrDefault(p => p.ProductID == orderDetail.ProductID);
                                if (product != null)
                                {
                                    product.StockQuantity += orderDetail.Quantity;
                                }
                            }
                        }
                        order.OrderStatusID = orderParam.orderStatusID;

                    }
                    // if the current status is "Completed" (3) or "Canceled" (4)
                    else
                    {
                        return BadRequest("The order is already " + (order.OrderStatusID == 3 ? "completed" : "canceled"));
                    }
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(("Something went wrong with the database: " + ex));
                }
            }
            return RedirectToAction("OrderList", "Order");
        }
    }
}
