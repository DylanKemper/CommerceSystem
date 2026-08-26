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
    }
}
