using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockify.Models;
using Stockify.Services;

namespace Stockify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Customer>> GetAll(){
            return Ok(_customerService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomer(int id){
            var customer = _customerService.GetById(id);
            if(customer == null){
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public ActionResult<Customer> Create(Customer customer) =>
        CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, _customerService.Add(customer));

        [HttpPut("{id}")]
        public IActionResult Update(int id, Customer customer)
        {
            if (id != customer.Id) return BadRequest();
            return _customerService.Update(customer) ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) =>
            _customerService.Delete(id) ? NoContent() : NotFound();
    }
}
