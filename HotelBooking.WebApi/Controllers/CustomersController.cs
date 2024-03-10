using System.Collections.Generic;
using HotelBooking.Core;
using Microsoft.AspNetCore.Mvc;


namespace HotelBooking.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController(IRepository<Customer> repos) : Controller
    {
        // GET: customers
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return repos.GetAll();
        }

    }
}
