# PosInformatique.Testing.Databases

**PosInformatique.Testing.Databases** is a set of tools for testing databases.
It simplifies writing and executing tests, helping ensure your database and data access code are reliable and bug-free.
It is ideal for developers who want to validate data access based on SQL Server code during their development.

This set of tools supports testing of the persistence layer based on SQL Server.
Any kind of data access framework can be used with these tools:
- Raw ADO .NET queries.
- Entity Framework.
- Dapper.
- ...

You can also use this tools to create and run integration tests with the
[Integration tests in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-8.0)
approach.

Since the version 2.0.0 this tools provide a comparer to compare the schema of two SQL databases.

## The approach of these tools

The main approach of these tools is to perform tests without using mocking or in-memory alternatives for ADO .NET code or Entity Framework `DbContext`, instead using a real SQL Server database.

Also, these tools offer simple way to compare two SQL databases to test migration script (or Entity Framework migration) when upgrading a database.

### Why is this approach recommended?

- Around 30% to 40% of the code in applications is located in the persistence layer or repository components. Because it is hard to test, developers often skip testing,
resulting in lower code coverage.
- When using a mock or in-memory approach for a `DbContext`, you don't truly test the Entity Framework mapping to your database, especially additional SQL constraints like nullability, uniqueness, foreign key cascades, etc.
You also miss technical behaviors like transactions, connection management, triggers, etc.
- When inserting data, it is crucial to ensure that the data in the columns are stored correctly (null/not null values, enum values to numerical values, custom or JSON serialized data, etc.).
- If you use Entity Framework, you can detect warnings/errors raised by the `DbContext` during the development.
- You perform test cases, meaning you write simple tests to validate small features instead of writing complex integration tests.
- When changing the schema of the database, it is important to test and have a *safeguard* to check that the migration script (or Entity Framework migration actions) will update the database to the expected schema.

## How to test a persistence layer

To perform tests of a persistence layer, the approach is straightforward using the Arrange/Act/Assert pattern:

Before each test (`TestMethod` or `Fact` methods):

1. Create an empty database with the SQL schema of the application.

   There are two ways to do this:
   - Deploy a DACPAC file (built by a SQL Server Database project).
   - Or create a database from a `DbContext` using Entity Framework.

2. Fill the tables with the sample data needed.

3. Execute the code (the method of the repository to be tested).

4. Assert the results of the executed code.

   - If the tested method returns data (performs a SELECT query), assert the returned objects using your favorite assertion framework (e.g., FluentAssertions).
   - If the method inserts, updates, or deletes data, assert the content of the tables to check that all data is stored correctly.

To write a test using this approach with the [PosInformatique.Testing.Databases](https://github.com/PosInformatique/PosInformatique.Testing.Databases) tools, see the [ Write tests to test the persistence layer](./docs/WriteTest.md) page.

## How to test database migration

To perform tests of a database migration, the approach is straightforward and required only a test which perform the following actions:

1. Create an empty database (*initial database*).

2. Create a secondary database with the targeted schema (*target database*).

   There are two ways to do this:
   - Deploy a DACPAC file (built by a SQL Server Database project).
   - Or create a database from a `DbContext` using Entity Framework.

3. Execute your database *migration code* on the *initial database*.

   Your database *migration code* can be:
   - A simple SQL script file.
   - An Entity Framework migration sets executed with the `MigrateAsync()` method.

4. Compare the two databases schemas (*initial* and *target*).

   If the database *migration code* works, the *initial* and *target* must have the same schema.

> **NB**: The initial database is not necessarily empty. It can be at a specific schema version X if we want to test the migration from version X to Y.

This approach does not test the migration of the data within the database. We can modify this process to inject some data in the first step to test it,
but writing the test can be time-consuming. By focusing on the schema migration of the database, you can verify at least 80-90% of your database
migration code. It's better than nothing and very useful for detecting issues during development or in a CI process!

To write a test using this approach with the [PosInformatique.Testing.Databases](https://github.com/PosInformatique/PosInformatique.Testing.Databases) tools, see the [Write tests to test database migration](./docs/WriteDatabaseMigrationTest.md) page.

## What do the PosInformatique.Testing.Databases tools provide?

Using the previous approach, the [PosInformatique.Testing.Databases](https://github.com/PosInformatique/PosInformatique.Testing.Databases) libraries allow you to:

- Easily deploy a database before each test execution.
  Database and schema creation can take a lot of time (around 5 to 10 seconds). The [PosInformatique.Testing.Databases](https://github.com/PosInformatique/PosInformatique.Testing.Databases) libraries physically create the database during the first test execution. For subsequent tests, all data is deleted in the database, which speeds up the test execution.

- Provide a simple syntax to fill the tables with sample data.

- Offer helpers to easily query and retrieve data from SQL tables (for assertions).

- Contain a comparer tool to check schema differences between two databases.

## NuGet packages

The [PosInformatique.Testing.Databases](https://github.com/PosInformatique/PosInformatique.Testing.Databases) tools are provided in two NuGet packages:

- [PosInformatique.Testing.Databases.SqlServer](https://www.nuget.org/packages/PosInformatique.Testing.Databases.SqlServer) NuGet package which contains:
  - Helpers to initialize SQL Server databases with sample data.
  - Helpers to easily query SQL Server databases.
  - Helpers to compare the schema of two SQL Server databases.

- [PosInformatique.Testing.Databases.SqlServer.Dac](https://www.nuget.org/packages/PosInformatique.Testing.Databases.SqlServer.Dac) NuGet package which contains:
  - Tools to deploy a SQL Server database using a DACPAC file before each test.

- [PosInformatique.Testing.Databases.SqlServer.EntityFramework](https://www.nuget.org/packages/PosInformatique.Testing.Databases.SqlServer.EntityFramework) NuGet package which contains:
  - Tools to deploy a SQL Server database using a DbContext.

## Samples / Demo

A complete sample solution is available in this repository inside the [samples](./samples) folder.

The solution contains the following sample projects:
- [DemoApp.Domain](./samples/DemoApp.Domain/DemoApp.Domain.csproj): Represents the domain of the application with a set of sample business entities.
- [DemoApp.DataAccessLayer](./samples/DemoApp.DataAccessLayer/DemoApp.DataAccessLayer.csproj): Represents a persistence layer with a set of repositories to test.
- [DemoApp.DataAccessLayer.Tests](./samples/DemoApp.DataAccessLayer.Tests/DemoApp.DataAccessLayer.Tests.csproj): Test project to test the [DemoApp.DataAccessLayer](./samples/DemoApp.DataAccessLayer/DemoApp.DataAccessLayer.csproj)
project using the [PosInformatique.Testing.Databases.SqlServer.EntityFramework](https://www.nuget.org/packages/PosInformatique.Testing.Databases.SqlServer.EntityFramework) package.

## Writing tests for a persistence layer

To write tests for a persistence layer, follow the [Write tests to test the persistence layer](./docs/WriteTest.md) documentation page, which explains the different steps to perform
using the [PosInformatique.Testing.Databases.SqlServer.EntityFramework](https://www.nuget.org/packages/PosInformatique.Testing.Databases.SqlServer.EntityFramework) library:

- [Create the SQL Server instance](./docs/WriteTest.md#create-the-sql-server-instance)
  - [Create the LocalDB instance](./docs/WriteTest.md#create-the-localdb-instance)
- [Create the tests project](./docs/WriteTest.md#create-the-tests-project)
- [Add the NuGet packages](./docs/WriteTest.md#add-the-nuget-packages)
- [Test class](./docs/WriteTest.md#test-class)
  - [Deploy a new instance of the database from a DbContext](./docs/WriteTest.md#deploy-a-new-instance-of-the-database-from-a-dbcontext)
  - [Parallel execution of the tests](./docs/WriteTest.md#parallel-execution-of-the-tests)
  - [Initialize the database data](./docs/WriteTest.md#initializes-the-data-of-the-database)
- [Write the tests for methods that retrieve data](./docs/WriteTest.md#write-the-tests-for-methods-that-retrieve-data)
- [Write the tests for methods that update the data](./docs/WriteTest.md#write-the-tests-for-methods-that-update-the-data)
- [Execute the tests](./docs/WriteTest.md#execute-the-tests)
- [Check the database state after a test has failed](./docs/WriteTest.md#check-the-database-state-after-an-test-has-been-failed)

## Writing test to check database migration

To write an test to check the migration of database, follow the [Write tests to test database migration](./docs/WriteDatabaseMigrationTest.md)
documentation page.

For Entity Framework migration:
- [Create the SQL Server instance](./docs/WriteTest.md#create-the-sql-server-instance)
  - [Create the LocalDB instance](./docs/WriteTest.md#create-the-localdb-instance)
- [Entity Framework migration approach](./docs/WriteDatabaseMigrationTest.md#entity-framework-migration-approach)
  - [Create the tests project](./docs/WriteDatabaseMigrationTest.md#create-the-tests-project)
  - [Add the NuGet packages](./docs/WriteDatabaseMigrationTest.md#add-the-nuget-packages)
  - [Write test to check the migration of the database](./docs/WriteDatabaseMigrationTest.md#write-test-to-check-the-migration-of-the-database)
  - [Check the report details of the `SqlServerDatabaseComparer` tool](./docs/WriteDatabaseMigrationTest.md#check-the-report-details-of-the-sqlserverdatabasecomparer-tool)
