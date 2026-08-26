using Stockify.Models;

namespace Stockify.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly List<Customer> _customers = new();     // In-memory list to store customers
        private int _nextId = 1;    // To generate unique IDs for new customers

        private readonly IOrderService _orderService;

        public CustomerService(IOrderService orderService)
        {
            _orderService = orderService;
            _customers.Add(new Customer
            {
                Id = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe"
            });
        }

        public Customer Add(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            customer.Id = _nextId++;
            _customers.Add(customer);
            return customer;
        }

        public bool Delete(int id)
        {
            if (_orderService.GetByCustomerId(id).Any())
            {
                throw new InvalidOperationException("Cannot delete customer with existing orders.");
            }

            var existing = GetById(id);
            if (existing == null)
            {
                return false;
            }
            return _customers.Remove(existing);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customers;
        }

        public Customer? GetById(int id)
        {
            return _customers.FirstOrDefault(c => c.Id == id);
        }

        public bool Update(Customer customer)
        {
            var existingCustomer = GetById(customer.Id);
            if (existingCustomer == null)
                return false;

            existingCustomer.Name = customer.Name;
            existingCustomer.Email = customer.Email;
            existingCustomer.UserName = customer.UserName;
            return true;
        }
    }
}