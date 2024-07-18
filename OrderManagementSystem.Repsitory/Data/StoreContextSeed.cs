using OrderManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderManagementSystem.Repsitory.Data
{
    public class StoreContextSeed
    {
        public async static Task SeedAsync(OrderManagementDbContext _dbcontext)
        {

            if (_dbcontext.Customers.Count() == 0)
            {
                var customerData = File.ReadAllText("../OrderManagementSystem.Repsitory/Data/DataSeed/Customers.json");
                var customers = JsonSerializer.Deserialize<List<Customer>>(customerData);

                if (customers?.Count > 0)
                {
                    foreach (var customer in customers)
                    {
                        _dbcontext.Set<Customer>().Add(customer);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }


            if (_dbcontext.Products.Count() == 0)
            {
                var productData = File.ReadAllText("../OrderManagementSystem.Repsitory/Data/DataSeed/Products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);

                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {
                        _dbcontext.Set<Product>().Add(product);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }



        }
    }
}
