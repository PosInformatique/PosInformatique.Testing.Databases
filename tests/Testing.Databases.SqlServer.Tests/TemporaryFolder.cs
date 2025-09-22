//-----------------------------------------------------------------------
// <copyright file="TemporaryFolder.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    public sealed class TemporaryFolder : IDisposable
    {
        private TemporaryFolder(string path)
        {
            this.Path = path;
        }

        public string Path { get; }

        public static TemporaryFolder Create()
        {
            var temporaryFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "PosInformatique.Testing.Databases.SqlServer.Tests", Guid.NewGuid().ToString());

            Directory.CreateDirectory(temporaryFolder);

            return new TemporaryFolder(temporaryFolder);
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(this.Path, true);
            }
            catch (IOException)
            {
                // Ignore the errors.
            }
        }
    }
}
