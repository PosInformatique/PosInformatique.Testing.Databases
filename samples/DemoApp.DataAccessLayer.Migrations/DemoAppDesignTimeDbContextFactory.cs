//-----------------------------------------------------------------------
// <copyright file="DemoAppDesignTimeDbContextFactory.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.DemoApp.DataAccessLayer.Migrations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class DemoAppDesignTimeDbContextFactory : IDesignTimeDbContextFactory<DemoAppDbContext>
    {
        public DemoAppDesignTimeDbContextFactory()
        {
        }

        public DemoAppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DemoAppDbContext>()
                .UseSqlServer(b => b.MigrationsAssembly(this.GetType().Assembly.GetName().Name));

            return new DemoAppDbContext(builder.Options);
        }
    }
}
