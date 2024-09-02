# PosInformatique.UnitTests.Databases
PosInformatique.UnitTests.Databases is a set of tools for unit testing databases.
It simplifies writing and executing tests, helping ensure your database and data access code is reliable and bug-free.
Ideal for developers who want to validate data access based on SQL Server code during their development.

This set of tools support to perform unit tests of the data access layer based on SQL Server. Any kind of the
data access framework can be used with theses tools:
- Raw ADO .NET queries.
- Entity Framework
- Dapper
- ...

## The approach of this tools

The main approach of this tools is to perform unit tests without using mocking or in-memory the ADO .NET code or Entity Framework `DbContext` and using a real SQL Server database.

Why this approach is recommanded?
- Around 30% / 40% of the code is the applications is located in the Data Access layer / repositories components. Because it is hard to unit tests, developers do not perform unit tests
and 30% / 40% of code coverage.
- When using a mock or in-memory approach of a `DbContext` you don't really test the Entity Framework mapping to your database and specially additional SQL constraints like nullability, unicity, foreign key cascade,... And
also all technical behavior like transactions, opening/closing connections,...
- When inserting data, we want to be sure that the data in the columns are stored correctly (null / not null values, enum values to numerical values, customer or JSON serialized data,...)
- If you use Entity Framework, you can detect warnings / errors raised by the `DbContext` (and not in the logs when deploying the application).
- You perform unit tests cases, it is mean you write simple tests to test small feature instead to write a complex integration test.

## How to unit tests a Data Access Layer.

To perform unit tests of a Data Access Layer the approach is easily using the Arrange / Act / Assert pattern:

Before each units (`TestMethod` or `Fact` methods):
1. Create an empty database with the SQL schema of the application.

   There is two ways to do it:
   - Deploying a DACPAC file (built by an SQL Server Database project).
   - Or executing creating a database from a `DbContext` using Entity Framework.

1. Fill the tables with sample of data that we need.

1. Executing the code (the method of the repository to test).

1. Assert the results of the code executed.

   If the method tested returns data (perform a SELECT query), we assert the objects returned with your favorite assert framework (FluentAssertions for example).
   If the method insert, update or delete data, we assert the content of the tables to check that all the data is stored correctly.

To write an unit test using this approach with the [PosInformatique.UnitTests.Databases](https://github.com/PosInformatique/PosInformatique.UnitTests.Databases) tools
see the [Write unit tests to test the Data Access Layer](./docs/WriteUnitTests.md) page.

## What provides the PosInformatique.UnitTests.Databases tools?

Using the previous approach, the [PosInformatique.UnitTests.Databases](https://github.com/PosInformatique/PosInformatique.UnitTests.Databases) libraries allows to:

- Deploy easily a database before each unit tests execution.
  Database and the schema creation is an operation that can take lot of time (around 5 to 10 seconds), the [PosInformatique.UnitTests.Databases](https://github.com/PosInformatique/PosInformatique.UnitTests.Databases) libraries
  create physically the database at the first unit test execution. For the other unit tests execution, all the data are deleted in the database which allows to increase the speed of the unit tests execution.

- Provides a simple syntax to fill the tables with sample of data.

- Provides helpers to query and retrieve easily data in SQL tables (to perform assertions).

## NuGet packages

The [PosInformatique.UnitTests.Databases](https://github.com/PosInformatique/PosInformatique.UnitTests.Databases) are provided into two NuGet packages:
- [![Nuget](https://img.shields.io/nuget/v/PosInformatique.UnitTests.Databases.SqlServer)](https://www.nuget.org/packages/PosInformatique.UnitTests.Databases.SqlServer) which contains:
  - Tools to deploy a SQL Server database using a DACPAC file before each unit tests.
  - Helper to initialize the SQL Server databases with sample data.
  - Helper to query easily SQL Server databases.

- [![Nuget](https://img.shields.io/nuget/v/PosInformatique.UnitTests.Databases.SqlServer.EntityFramework)](https://www.nuget.org/packages/PosInformatique.UnitTests.Databases.SqlServer.EntityFramework) which contains:
  - Tools to deploy a SQL Server database using a DbContext.
  
  This package use and contains the previous [PosInformatique.UnitTests.Databases.SqlServer](https://www.nuget.org/packages/PosInformatique.UnitTests.Databases.SqlServer) NuGet package.

## Samples / Demo

A complete sample solution is available in this repository in the [./samples](./samples) folder.

The solution contains the following projects:
- [DemoApp.Domain](./samples/DemoApp.Domain/DemoApp.Domain.csproj): Represents the domain of the application with a set of sample business entities.
- [DemoApp.DataAccessLayer](./samples/DemoApp.DataAccessLayer/DemoApp.DataAccessLayer.csproj): Represents a Data Access Layer with a set of repository to unit tests.
- [DemoApp.DataAccessLayer.Tests](./samples/DemoApp.DataAccessLayer.Tests/DemoApp.DataAccessLayer.Tests.csproj): Unit tests projects to test the [DemoApp.DataAccessLayer](./samples/DemoApp.DataAccessLayer/DemoApp.DataAccessLayer.csproj)
project using the [PosInformatique.UnitTests.Databases.SqlServer](https://www.nuget.org/packages/PosInformatique.UnitTests.Databases.SqlServer) package.

## Write unit tests to test a Data Access Layer.

To write unit tests for a Data Access Layer follows [Write unit tests to test the Data Access Layer](./docs/WriteUnitTests.md) documentation page, which explain the different steps to perform
using the [PosInformatique.UnitTests.Databases.SqlServer.EntityFramework](https://www.nuget.org/packages/PosInformatique.UnitTests.Databases.SqlServer.EntityFramework) library:

- [Create the SQL Server instance](./docs/WriteUnitTests.md#create-the-sql-server-instance)
  - [Create the LocalDB instance](./docs/WriteUnitTests.md#create-the-localdb-instance)
- [Create an unit tests project](./docs/WriteUnitTests.md#create-the-unit-tests-project)
- [Add the NuGet packages](./docs/WriteUnitTests.md#add-the-nuget-packages)
- [Unit test class](./docs/WriteUnitTests.md#unit-test-class)
  - [Deploy a new instance of the database from a DbContext](./docs/WriteUnitTests.md#deploy-a-new-instance-of-the-database-from-a-dbcontext)
  - [Parallelism execution of the unit tests](./docs/WriteUnitTests.md#parallelism-execution-of-the-unit-tests)
  - [Initializes the data of the database](./docs/WriteUnitTests.md#initializes-the-data-of-the-database)
- [Write the unit tests for methods that retrieve data](./docs/WriteUnitTests.md#write-the-unit-tests-for-methods-that-retrieve-data)
- [Write the unit tests for methods that update the data](./docs/WriteUnitTests.md#write-the-unit-tests-for-methods-that-update-the-data)
- [Execute the unit tests](./docs/WriteUnitTests.md#execute-the-unit-tests)
- [Check the database state after an unit test has been failed](./docs/WriteUnitTests.md#check-the-database-state-after-an-unit-test-has-been-failed)
