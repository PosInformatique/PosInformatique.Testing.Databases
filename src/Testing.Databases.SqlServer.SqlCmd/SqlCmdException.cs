//-----------------------------------------------------------------------
// <copyright file="SqlCmdException.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer
{
    /// <summary>
    /// Occured when an error has been raised by the SQL Server <c>sqlcmd</c> tool.
    /// </summary>
    public class SqlCmdException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCmdException"/> class.
        /// </summary>
        public SqlCmdException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCmdException"/> class
        /// with the specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public SqlCmdException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCmdException"/> class
        /// with the specified <paramref name="message"/> and <paramref name="output"/>
        /// of the <c>sqlcmd</c> process.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="output">Output of the <c>sqlcmd</c> process.</param>
        public SqlCmdException(string message, string output)
            : base(message)
        {
            this.Output = output;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCmdException"/> class
        /// with the specified <paramref name="message"/> raised by
        /// an other <paramref name="innerException"/>.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Previous <see cref="Exception"/> which raised the <see cref="SqlCmdException"/>.</param>
        public SqlCmdException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets the output of the <c>sqlcmd</c> execution.
        /// </summary>
        public string? Output { get; }
    }
}
