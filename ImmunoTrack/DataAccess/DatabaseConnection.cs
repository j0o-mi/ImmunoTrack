// Folder: DataAccess/DatabaseConnection.cs
using System;
using Microsoft.Data.SqlClient;

namespace ImmunoTrack.DataAccess
{
    /// <summary>
    /// The ONE place that knows how to reach the database.
    /// If the server name ever changes, edit ConnectionString here only.
    /// </summary>
    public static class DatabaseConnection
    {
        private const string ConnectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=ImmunoTrackDB;" +
            "Trusted_Connection=True;TrustServerCertificate=True;";

        // Every data access class calls this, uses the connection inside a
        // "using" block, and it is closed automatically afterwards.
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        // Quick check used to confirm the app can reach the database.
        public static bool TestConnection(out string message)
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    message = "Connected to ImmunoTrackDB successfully.";
                    return true;
                }
            }
            catch (SqlException ex)
            {
                message = "Database connection failed: " + ex.Message;
                return false;
            }
        }
    }
}
