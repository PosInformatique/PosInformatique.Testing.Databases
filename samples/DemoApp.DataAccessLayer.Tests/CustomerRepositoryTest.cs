//-----------------------------------------------------------------------
// <copyright file="CustomerRepositoryTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.DemoApp.DataAccessLayer.Tests
{
    using FluentAssertions;
    using PosInformatique.Testing.Databases.SqlServer;

    [Collection(DatabaseName)]
    public class CustomerRepositoryTest : IClassFixture<SqlServerDatabaseInitializer>
    {
        private const string DatabaseName = nameof(CustomerRepositoryTest);

        private readonly SqlServerDatabase database;

        public CustomerRepositoryTest(SqlServerDatabaseInitializer initializer)
        {
            // Deploy the database using Entity Framework
            using var dbContext = new DemoAppDbContext(DatabaseTestsConnectionStrings.CreateDbContextOptions<DemoAppDbContext>(DatabaseName));

            this.database = initializer.Initialize(dbContext);

            // Insert sample data for the Customer table.
            // - Here we let SQL Server auto increments and set the ID with an IDENTITY value (The ID will be automatically define to "1" here).
            this.database.InsertInto("Customer", disableIdentityInsert: false, new { FirstName = "John", LastName = "DOE", Revenue = 110.50 });

            // - Here we force to set the ID of the customer to 15.
            this.database.InsertInto("Customer", disableIdentityInsert: true, new { Id = 15, FirstName = "Marcel", LastName = "DUPONT", Revenue = 4852.45 });

            // - Here, to simplify the syntax (recommanded approach) we use an extension method in the tests project.
            //   Using extension methods, make the code more readable.
            //   Also, we recommand to force to set the IDENTITY column values explicit to avoid
            //   to update lot of code if you delete some rows later...
            this.database.InsertCustomer(id: 20, firstName: "Andres", lastName: "GARCIA");
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            using var dbContext = new DemoAppDbContext(DatabaseTestsConnectionStrings.CreateDbContextOptions<DemoAppDbContext>(DatabaseName));

            var repository = new CustomerRepository(dbContext);

            var customer = new Customer("Hector", "BARBOSSA")
            {
                Revenue = 12.34M,
            };

            // Act
            await repository.CreateAsync(customer);

            // Assert
            // Here we must query the table and check that the customer has been inserted with correct values.
            // Advice: Tests only the "raw" values in the SQL Server table. Don't use Entity Framework or other framework / helper.
            //         and don't wrap the results in other class.
            //         To summarize: Test the content of the rows / columns !
            var table = this.database.ExecuteQuery($"SELECT * FROM Customer WHERE Id = '{customer.Id}'");

            table.Rows.Should().HaveCount(1);   // Advice: Event the ID is unique, check there is no duplicated entry. To protect against refactory unicity changes.

            table.Rows[0]["FirstName"].Should().Be("Hector");
            table.Rows[0]["LastName"].Should().Be("BARBOSSA");
            table.Rows[0]["Revenue"].Should().Be(12.34);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Arrange
            using var dbContext = new DemoAppDbContext(DatabaseTestsConnectionStrings.CreateDbContextOptions<DemoAppDbContext>(DatabaseName));

            var repository = new CustomerRepository(dbContext);

            // Act
            // Delete the customer with ID = 15 (Marcel / DUPONT)
            await repository.DeleteAsync(15);

            // Assert
            // Here we must query the table and check that the customer row does not exists.
            var table = this.database.ExecuteQuery($"SELECT * FROM Customer WHERE Id = 15");

            table.Rows.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteAsync_CustomerNotFound()
        {
            // Arrange
            using var dbContext = new DemoAppDbContext(DatabaseTestsConnectionStrings.CreateDbContextOptions<DemoAppDbContext>(DatabaseName));

            var repository = new CustomerRepository(dbContext);

            // Act
            // Delete the customer with ID = 99 (Not found)
            var invoking = repository.Invoking(r => r.DeleteAsync(99));

            // Assert
            await invoking.Should().ThrowExactlyAsync<CustomerNotFoundException>()
                .WithMessage("No customer found with the '99' identifier.");
        }

        [Fact]
        public async Task GetAsync()
        {
            // Arrange
            using var dbContext = new DemoAppDbContext(DatabaseTestsConnectionStrings.CreateDbContextOptions<DemoAppDbContext>(DatabaseName));

            var repository = new CustomerRepository(dbContext);

            // Act
            // Gets the customer with the ID = 15 (Marcel / DUPONT)
            var customer = await repository.GetAsync(15);

            // Assert
            // Here we must assert the object returned by the GetAsync() method.
            customer.Id.Should().Be(15);
            customer.FirstName.Should().Be("Marcel");
            customer.LastName.Should().Be("DUPONT");
            customer.Revenue.Should().Be(4852.45M);
        }

        [Fact]
        public async Task GetAsync_CustomerNotFound()
        {
            // Arrange
            using var dbContext = new DemoAppDbContext(DatabaseTestsConnectionStrings.CreateDbContextOptions<DemoAppDbContext>(DatabaseName));

            var repository = new CustomerRepository(dbContext);

            // Act
            // Gets the customer with the ID = 99 (Not found)
            var invoking = repository.Invoking(r => r.GetAsync(99));

            // Assert
            await invoking.Should().ThrowExactlyAsync<CustomerNotFoundException>()
                .WithMessage("No customer found with the '99' identifier.");
        }

        [Fact]
        public async Task ResetRevenue()
        {
            // Arrange
            using var dbContext = new DemoAppDbContext(DatabaseTestsConnectionStrings.CreateDbContextOptions<DemoAppDbContext>(DatabaseName));

            var repository = new CustomerRepository(dbContext);

            // Act
            // Call the ResetRevenue() method.
            await repository.ResetRevenueAsync();

            // Assert
            // Check that all the revenue of all customers has been set to zero.
            var table = this.database.ExecuteQuery($"SELECT * FROM Customer");

            table.Rows.Should().AllSatisfy(r => r["Revenue"].Should().Be(0));
        }
    }
}