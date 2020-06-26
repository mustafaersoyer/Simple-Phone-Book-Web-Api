using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhoneBookBackend.Models;

namespace PhoneBookBackend.Data
{
    public class pbContext : DbContext
    {
        public pbContext(DbContextOptions<pbContext> options)
           : base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; }
    }
}
