//-----------------------------------------------------------------------
// <copyright file="DemoAppDatabaseExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.DemoApp.DataAccessLayer.Tests
{
    using PosInformatique.Testing.Databases.SqlServer;

    /// <summary>
    /// Contains static methods to insert data in the database.
    /// These kind of extensions methods is not required, but simplify the
    /// code to write using the <see cref="SqlServerDatabaseExtensions.InsertInto{T}(SqlServerDatabase, string, bool, T[])"/>
    /// methods. See the constructor of the <see cref="CustomerRepositoryTest(SqlServerDatabaseInitializer)"/>
    /// to see an example usage.
    /// </summary>
    public static class DemoAppDatabaseExtensions
    {
        public static void InsertCustomer(this SqlServerDatabase database, int id, string firstName, string lastName, decimal revenue = 0)
        {
            database.InsertInto("Customer", true, new
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Revenue = revenue,
            });
        }
    }
}
