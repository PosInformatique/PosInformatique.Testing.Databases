//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.DemoApp.DataAccessLayer.Migrations
{
    using Microsoft.EntityFrameworkCore;

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
}
