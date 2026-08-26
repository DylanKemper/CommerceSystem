using Stockify.Models;

namespace Stockify.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();
        Customer? GetById(int id);
        Customer Add(Customer customer);
        bool Update(Customer customer); 
        bool Delete(int id);
    }
}