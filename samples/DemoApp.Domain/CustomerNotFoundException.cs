//-----------------------------------------------------------------------
// <copyright file="CustomerNotFoundException.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.DemoApp
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException()
            : base()
        {
        }

        public CustomerNotFoundException(string message)
            : base(message)
        {
        }

        public CustomerNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
