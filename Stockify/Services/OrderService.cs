using Stockify.Models;

namespace Stockify.Services
{
    public class OrderService : IOrderService
    {
        private readonly List<Order> _orders = new();   // In-memory list to store orders
        private int _nextOrderId = 1;
        private int _nextItemId = 1;

        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;

        public OrderService(ICustomerService customerService, IProductService productService)
        {
            _customerService = customerService;
            _productService = productService;
        }

        public Order Add(Order order)
        {
            // Customer must exist before adding an order
            if (_customerService.GetById(order.CustomerId) == null)
            {
                throw new ArgumentException($"Customer with ID {order.CustomerId} does not exist.");
            }

            order.Id = _nextOrderId++;
            order.OrderDate = DateTime.Now;     // Set the order date to the current date and time

            foreach (var orderItem in order.Items)
            {
                // Vailidate FK - all products must exist before adding an order item
                var product = _productService.GetById(orderItem.ProductId);
                if (product == null)
                {
                    throw new ArgumentException($"Product with ID {orderItem.ProductId} does not exist.");
                }

                orderItem.Id = _nextItemId++;
                orderItem.OrderId = order.Id;
                orderItem.UnitPrice = product.Price;   // Set the unit price to the product's price
            }

            _orders.Add(order);
            return order;
        }

        public bool Delete(int id)
        {
            var existingOrder = GetById(id);
            if (existingOrder == null)
            {
                return false;
            }

            _orders.Remove(existingOrder);
            return true;
        }

        public IEnumerable<Order> GetAll()
        {
            return _orders;
        }

        public IEnumerable<Order> GetByCustomerId(int customerId)
        {
            return _orders.Where(o => o.CustomerId == customerId);
        }

        public Order? GetById(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            return order;
        }

        public bool Update(Order order)
        {
            // Check that the order exists
            var existingOrder = GetById(order.Id);
            if (existingOrder == null)
            {
                return false;
            }

            // Update the order properties
            existingOrder.CustomerId = order.CustomerId;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.Items = order.Items;

            return true;
        }
    }
}
