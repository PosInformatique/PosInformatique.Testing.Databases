//-----------------------------------------------------------------------
// <copyright file="SqlCmdExceptionTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    public class SqlCmdExceptionTest
    {
        [Fact]
        public void Constructor()
        {
            var exception = new SqlCmdException();

            exception.Message.Should().Be("Exception of type 'PosInformatique.Testing.Databases.SqlServer.SqlCmdException' was thrown.");
            exception.Output.Should().BeNull();
            exception.InnerException.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithMessage()
        {
            var exception = new SqlCmdException("The message");

            exception.Message.Should().Be("The message");
            exception.Output.Should().BeNull();
            exception.InnerException.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithMessageAndOutput()
        {
            var exception = new SqlCmdException("The message", "The output");

            exception.Message.Should().Be("The message");
            exception.Output.Should().Be("The output");
            exception.InnerException.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException()
        {
            var innerException = new FormatException("The inner exception");
            var exception = new SqlCmdException("The message", innerException);

            exception.Message.Should().Be("The message");
            exception.Output.Should().BeNull();
            exception.InnerException.Should().BeSameAs(innerException);
        }
    }
}