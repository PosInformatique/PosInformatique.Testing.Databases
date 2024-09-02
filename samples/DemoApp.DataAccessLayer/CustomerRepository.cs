//-----------------------------------------------------------------------
// <copyright file="CustomerRepository.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.DemoApp.DataAccessLayer
{
    using Microsoft.EntityFrameworkCore;

    public class CustomerRepository
    {
        private readonly DemoAppDbContext dbContext;

        public CustomerRepository(DemoAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Customer customer)
        {
            await this.dbContext.Customers.AddAsync(customer);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customerFound = await this.dbContext.Customers.SingleOrDefaultAsync(c => c.Id == id);

            if (customerFound is null)
            {
                throw new CustomerNotFoundException($"No customer found with the '{id}' identifier.");
            }

            this.dbContext.Customers.Remove(customerFound);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<Customer> GetAsync(int id)
        {
            var customerFound = await this.dbContext.Customers.SingleOrDefaultAsync(c => c.Id == id);

            if (customerFound is null)
            {
                throw new CustomerNotFoundException($"No customer found with the '{id}' identifier.");
            }

            return customerFound;
        }

        public async Task ResetRevenueAsync()
        {
            await this.dbContext.Customers.ExecuteUpdateAsync(c => c.SetProperty(e => e.Revenue, 0));
        }
    }
}
