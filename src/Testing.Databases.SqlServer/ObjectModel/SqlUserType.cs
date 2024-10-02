//-----------------------------------------------------------------------
// <copyright file="SqlUserType.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases
{
    /// <summary>
    /// Represents a user type in SQL database.
    /// </summary>
    public sealed class SqlUserType : SqlObject
    {
        internal SqlUserType(string name, short maxLength)
        {
            this.Name = name;
            this.MaxLength = maxLength;
        }

        /// <summary>
        /// Gets the name of the user type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the max length of the type.
        /// </summary>
        public short MaxLength { get; }

        /// <summary>
        /// Gets a value indicating whether is the type is nullable.
        /// </summary>
        public bool IsNullable { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether is the type is a table.
        /// </summary>
        public bool IsTableType { get; internal set; }

        /// <inheritdoc />
        public override TResult Accept<TResult>(ISqlObjectVisitor<TResult> visitor) => visitor.Visit(this);

        /// <inheritdoc cref="Name"/>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
