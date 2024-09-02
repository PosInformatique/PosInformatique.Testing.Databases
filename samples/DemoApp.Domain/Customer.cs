//-----------------------------------------------------------------------
// <copyright file="Customer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.DemoApp
{
    public class Customer
    {
        public Customer(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public int Id { get; set; }

        public string FirstName { get; }

        public string LastName { get; }

        public decimal Revenue { get; set; }
    }
}
