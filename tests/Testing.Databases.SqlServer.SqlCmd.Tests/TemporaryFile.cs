//-----------------------------------------------------------------------
// <copyright file="TemporaryFile.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    internal sealed class TemporaryFile : IDisposable
    {
        private TemporaryFile(string fileName)
        {
            this.FileName = fileName;
        }

        public string FileName { get; }

        public static TemporaryFile Create()
        {
            var temporaryFileName = Path.GetTempFileName();

            return new TemporaryFile(temporaryFileName);
        }

        public void Dispose()
        {
            try
            {
                if (File.Exists(this.FileName))
                {
                    File.Delete(this.FileName);
                }
            }
            catch (IOException)
            {
                // Ignore the errors.
            }
        }
    }
}
