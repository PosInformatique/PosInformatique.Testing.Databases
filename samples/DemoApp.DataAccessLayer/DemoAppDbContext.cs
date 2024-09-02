//-----------------------------------------------------------------------
// <copyright file="DemoAppDbContext.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.DemoApp.DataAccessLayer
{
    using Microsoft.EntityFrameworkCore;

    public class DemoAppDbContext : DbContext
    {
        public DemoAppDbContext(DbContextOptions<DemoAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers => this.Set<Customer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .UseIdentityColumn();
                entity.Property(e => e.FirstName)
                    .HasColumnType("varchar(50)");
                entity.Property(e => e.LastName)
                    .HasColumnType("varchar(50)");
                entity.Property(e => e.Revenue)
                    .HasColumnType("decimal(10, 2)");
            });
        }
    }
}
