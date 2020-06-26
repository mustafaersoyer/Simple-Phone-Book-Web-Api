using Newtonsoft.Json;
using PhoneBookBackend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace PhoneBookBackend.Data
{
    public class DbInitiliazer
    {
        public static void Initialize(pbContext context)
        {
            if (context.Customer.Any())
            {
                return;
            }

            var users = new Customer[] { };
            using (StreamReader r = new StreamReader("MOCK_CUSTOMERS.json"))
            {
                string json = r.ReadToEnd();
                
                dynamic array = JsonConvert.DeserializeObject(json);
                foreach (var item in array)
                {
                    var customer = new Customer { FirstName = item.first_name, LastName = item.last_name, Email = item.email, Phone = item.phone, Gender =item.gender };
                    context.Customer.Add(customer);

                }
            }
           


            context.SaveChanges();
        }
    }
}
