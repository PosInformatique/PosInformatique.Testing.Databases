# Write tests to test database migration

This section explain how to write an test to test the database migration. The code samples
used here can be found in the [PosInformatique.Testing.Databases.Samples](../samples/PosInformatique.Testing.Databases.Samples.sln)
solution inside the `samples` directory of the repository.

Before continuing, ensures that you followed these steps in the
[Write tests to test the Data Access Layer](./WriteTest.md) section:

- [Create the SQL Server instance](./WriteTest.md#create-the-sql-server-instance)
  - [Create the LocalDB instance](./WriteTest.md#create-the-localdb-instance)

## Entity Framework migration approach

In this subsection, we will write a test to check that our `DemoAppDbContext` migrations work.
Do not hesitate to read the official [Microsoft Entity Framework migrations documentation](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/) to learn how to generate Entity Framework code.

To execute the migration of our database, we create a simple console application that executes the Entity Framework generated migration code.
The connection string of the database to upgrade will be received from the command line of the console application.

Here is the code for the `Main()` method of the console application that performs the migration of the database.

```csharp
public static class Program
{
    public static async Task Main(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DemoAppDbContext>();
        optionsBuilder.UseSqlServer(args[0], b =>
        {
            b.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
        });

        using (var context = new DemoAppDbContext(optionsBuilder.Options))
        {
            await context.Database.MigrateAsync();
        }
    }
}
```

In the following section, we will see the step to write a test to test the console application which perform
the migration of the database.

### Create the tests project

To test the console application, we create an `xUnit Test Project` in Visual Studio which reference our console application project
that contains the `Main()` method.

### Add the NuGet packages

In the test project, we add the
[PosInformatique.Testing.Databases.SqlServer.EntityFramework](https://www.nuget.org/packages/PosInformatique.Testing.Databases.SqlServer.EntityFramework)
NuGet package.

This package will allow us to:
- Create an empty database (*initial database*).
- Create the *target database* using the `DemoAppDbContext`.
- Compare the *initial* and *target* databases to check if the console application migrates the *initial* database to the *target* database schema.

### Write test to check the migration of the database

To test if the migration code of the database works, we create a test `MigrationWithConsoleApp` which perform the following steps:
- Create an empty `DemoApp_InitialDatabase` SQL Server database.
- Create a SQL Server database `DemoApp_TargetDatabase` using the `DemoAppDbContext`
- Call the `Main()` method of the console application with the connection string.
- Compare the *initial* and *target* database and ensures there is no schema differences.

```csharp
public class DatabaseMigrationTest
{
    [Fact]
    public async Task MigrationWithConsoleApp()
    {
        const string InitialDatabaseName = "DemoApp_InitialDatabase";
        const string TargetDatabaseName = "DemoApp_TargetDatabase";

        var server = new SqlServer($"Data Source=(localdb)\\DemoApp; Integrated Security=True");

        // Create the initial database
        var initialDatabase = Task.Run(() => server.CreateEmptyDatabase(InitialDatabaseName));

        // Create the target database
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<DemoAppDbContext>()
            .UseSqlServer();
        var dbContext = new DemoAppDbContext(dbContextOptionsBuilder.Options);

        var targetDatabase = Task.Run(() => server.CreateDatabaseAsync(TargetDatabaseName, dbContext));

        // Wait both task
        await Task.WhenAll(initialDatabase, targetDatabase);

        // Call the console application to perform migration of the "DemoApp_InitialDatabase"
        var args = new[]
        {
            initialDatabase.Result.ConnectionString,
        };

        await Program.Main(args);

        // Compare the initial and target database
        var comparerOptions = new SqlDatabaseComparerOptions()
        {
            ExcludedTables =
            {
                { "__EFMigrationsHistory" },
            },
        };

        var differences = await SqlServerDatabaseComparer.CompareAsync(initialDatabase.Result, targetDatabase.Result, comparerOptions);

        differences.IsIdentical.Should().BeTrue(differences.ToString());
    }
}
```

> NB: When using Entity Framework migration a `__EFMigrationsHistory` is automatically added to record
all the migration executed. This is why we exclude this table in the `SqlDatabaseComparerOptions`.

When comparing two databases, the differences found are explicitly defined in the object model `SqlDatabaseComparisonResults`.
The `IsIdentical` allows to determine if there is the database have the same schema. In otherwise we will have to explore in detail the
`SqlDatabaseComparisonResults` object properties to determine the difference.

Instead of querying recursively the difference between the two database using the `SqlDatabaseComparisonResults`, the `ToString()`
method can be and contains a detailed report of the difference. This why in the previous code, we use in a `because` parameter of the
FluentAssertions `BeTrue()` assertion, to display the detailed report when the test has been failed.

### Check the report details of the `SqlServerDatabaseComparer` tool.

Here we will show what happen when a difference in the expected database schema has been (in other words, when a developer
did bug in the migration script 😁...).

To test the output of the `SqlDatabaseComparisonResults.ToString()` method, in the `InitialVersion`
migration class (`20240926074452_Initial-Version.cs` file) remove the `LastName` property which represents
the *bug* in other migration script.

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.CreateTable(
        name: "Customer",
        columns: table => new
        {
            Id = table.Column<int>(type: "int", nullable: false)
                .Annotation("SqlServer:Identity", "1, 1"),
            FirstName = table.Column<string>(type: "varchar(50)", nullable: false),
            // Do not create the "LastName" column.
            // LastName = table.Column<string>(type: "varchar(50)", nullable: false),
            Revenue = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_Customer", x => x.Id);
        });
}
```

Run the `MigrationWithConsoleApp` test and you will see that an exception is occured (the test failed has been failed).
If you look at the output of the test, you will the following FluentAssertion error message:

```
Expected differences.IsIdentical to be true because
------ Tables ------
- dbo.Customer
  ------ Columns ------
  - LastName (Missing in the source)
```

As you can see the `SqlServerDatabaseComparer` tool detect that the column `LastName` is missing
in the *source* database (`DemoApp_InitialDatabase`).