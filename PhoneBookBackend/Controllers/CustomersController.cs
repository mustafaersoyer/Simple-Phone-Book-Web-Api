using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PhoneBookBackend.Data;
using Newtonsoft.Json.Linq;
using PhoneBookBackend.Models;

namespace PhoneBookBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly pbContext _context;

        public CustomersController(pbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> Get(string filter = "", string range = "", string sort = "")
        {
            var entityQuery = _context.Customer.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                var filterVal = (JObject)JsonConvert.DeserializeObject(filter);
                foreach (var f in filterVal)
                {          
                    entityQuery = entityQuery.Where(x =>x.FirstName.Contains(f.Value.ToString()) || x.LastName.Contains(f.Value.ToString()) || x.Phone.Contains(f.Value.ToString()) || x.Gender.Contains(f.Value.ToString()) || x.Email.Contains(f.Value.ToString()));
                }
            }
            var count = entityQuery.Count();

           

            var from = 0;
            var to = 0;
            if (!string.IsNullOrEmpty(range))
            {
                var rangeVal = JsonConvert.DeserializeObject<List<int>>(range);
                from = rangeVal.First();
                to = rangeVal.Last();
                entityQuery = entityQuery.Skip(from).Take(to - from + 1);
            }

            Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            Response.Headers.Add("Content-Range", $"{typeof(Customer).Name.ToLower()} {from}-{to}/{count}");
            return await entityQuery.ToListAsync();
        }


        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

    

       [HttpPost]
        public async Task<ActionResult<Customer>> Post(Customer entity)
        {
            _context.Customer.Add(entity);
            await _context.SaveChangesAsync();
            return Ok(entity);

        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}
