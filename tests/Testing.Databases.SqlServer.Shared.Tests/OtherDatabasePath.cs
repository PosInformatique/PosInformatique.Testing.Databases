//-----------------------------------------------------------------------
// <copyright file="OtherDatabasePath.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Databases.SqlServer.Tests
{
    public sealed class OtherDatabasePath : IDisposable
    {
        private readonly bool deleteOnDispose;

        private OtherDatabasePath(string path, bool deleteOnDispose)
        {
            this.Path = path;
            this.deleteOnDispose = deleteOnDispose;
        }

        public string Path { get; }

        public static OtherDatabasePath Create()
        {
            var otherDataPath = Environment.GetEnvironmentVariable("SQL_SERVER_UNIT_TESTS_OTHER_DATA_PATH");

            var deleteOnDispose = false;

            if (otherDataPath is null)
            {
                otherDataPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "PosInformatique.Testing.Databases.SqlServer.Tests", Guid.NewGuid().ToString());

                Directory.CreateDirectory(otherDataPath);

                deleteOnDispose = true;
            }

            return new OtherDatabasePath(otherDataPath, deleteOnDispose);
        }

        public void Dispose()
        {
            if (this.deleteOnDispose)
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
}
