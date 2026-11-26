// Folder: DataAccess/HealthWorkerDataAccess.cs
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using ImmunoTrack.Models;

namespace ImmunoTrack.DataAccess
{
    /// <summary>The salt and hash stored for a health worker (used by the login check).</summary>
    public class StoredCredentials
    {
        public int HealthWorkerID { get; set; }
        public string PasswordSalt { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    }

    /// <summary>
    /// Talks to the HealthWorkers table. The normal list/search queries never
    /// read the password columns; only GetCredentials does (for login).
    /// </summary>
    public class HealthWorkerDataAccess
    {
        private const string SelectColumns =
            "HealthWorkerID, FirstName, MiddleName, LastName, ContactNumber, Position, LicenseNo, Username, Role";

        public List<HealthWorker> GetAll()
        {
            string sql = $"SELECT {SelectColumns} FROM HealthWorkers ORDER BY LastName, FirstName";
            return ReadWorkers(sql, null);
        }

        public HealthWorker? GetById(int healthWorkerId)
        {
            string sql = $"SELECT {SelectColumns} FROM HealthWorkers WHERE HealthWorkerID = @HealthWorkerID";
            var parameters = new List<SqlParameter> { new SqlParameter("@HealthWorkerID", healthWorkerId) };
            List<HealthWorker> result = ReadWorkers(sql, parameters);
            return result.Count > 0 ? result[0] : null;
        }

        // SEARCH: by name or username (partial match)
        public List<HealthWorker> Search(string text)
        {
            string sql = $@"SELECT {SelectColumns} FROM HealthWorkers
                            WHERE (FirstName + ' ' + LastName) LIKE @Search
                               OR (FirstName + ' ' + ISNULL(MiddleName + ' ', '') + LastName) LIKE @Search
                               OR Username LIKE @Search
                            ORDER BY LastName, FirstName";
            var parameters = new List<SqlParameter> { new SqlParameter("@Search", "%" + text + "%") };
            return ReadWorkers(sql, parameters);
        }

        // Is this username already used by a DIFFERENT health worker?
        public bool UsernameExists(string username, int excludeHealthWorkerId)
        {
            const string sql = @"SELECT COUNT(*) FROM HealthWorkers
                                 WHERE Username = @Username AND HealthWorkerID <> @ExcludeID";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@ExcludeID", excludeHealthWorkerId);
                connection.Open();
                return (int)command.ExecuteScalar() > 0;
            }
        }

        // CREATE: returns the new HealthWorkerID
        public int Add(HealthWorker worker, string passwordSalt, byte[] passwordHash)
        {
            const string sql = @"INSERT INTO HealthWorkers
                                    (FirstName, MiddleName, LastName, ContactNumber, Position, LicenseNo,
                                     Username, PasswordSalt, PasswordHash, Role)
                                 VALUES
                                    (@FirstName, @MiddleName, @LastName, @ContactNumber, @Position, @LicenseNo,
                                     @Username, @PasswordSalt, @PasswordHash, @Role);
                                 SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddProfileParameters(command, worker);
                command.Parameters.Add("@PasswordSalt", SqlDbType.VarChar, 32).Value = passwordSalt;
                command.Parameters.Add("@PasswordHash", SqlDbType.VarBinary, 32).Value = passwordHash;
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        // UPDATE the profile (does NOT touch the password)
        public void Update(HealthWorker worker)
        {
            const string sql = @"UPDATE HealthWorkers
                                 SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName,
                                     ContactNumber = @ContactNumber, Position = @Position,
                                     LicenseNo = @LicenseNo, Username = @Username, Role = @Role
                                 WHERE HealthWorkerID = @HealthWorkerID";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddProfileParameters(command, worker);
                command.Parameters.AddWithValue("@HealthWorkerID", worker.HealthWorkerID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // UPDATE only the password
        public void UpdatePassword(int healthWorkerId, string passwordSalt, byte[] passwordHash)
        {
            const string sql = @"UPDATE HealthWorkers
                                 SET PasswordSalt = @PasswordSalt, PasswordHash = @PasswordHash
                                 WHERE HealthWorkerID = @HealthWorkerID";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@PasswordSalt", SqlDbType.VarChar, 32).Value = passwordSalt;
                command.Parameters.Add("@PasswordHash", SqlDbType.VarBinary, 32).Value = passwordHash;
                command.Parameters.AddWithValue("@HealthWorkerID", healthWorkerId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // DELETE (error 547 if the worker has recorded vaccinations)
        public void Delete(int healthWorkerId)
        {
            const string sql = "DELETE FROM HealthWorkers WHERE HealthWorkerID = @HealthWorkerID";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@HealthWorkerID", healthWorkerId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // For LOGIN: the stored salt + hash for a username (null if no such user)
        public StoredCredentials? GetCredentials(string username)
        {
            const string sql = @"SELECT HealthWorkerID, PasswordSalt, PasswordHash
                                 FROM HealthWorkers WHERE Username = @Username";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new StoredCredentials
                    {
                        HealthWorkerID = (int)reader["HealthWorkerID"],
                        PasswordSalt = (string)reader["PasswordSalt"],
                        PasswordHash = (byte[])reader["PasswordHash"]
                    };
                }
            }
        }

        // ---------- private helpers ----------

        private List<HealthWorker> ReadWorkers(string sql, List<SqlParameter>? parameters)
        {
            var workers = new List<HealthWorker>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters.ToArray());

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        workers.Add(MapWorker(reader));
                }
            }

            return workers;
        }

        private static HealthWorker MapWorker(SqlDataReader reader)
        {
            return new HealthWorker(
                (int)reader["HealthWorkerID"],
                (string)reader["FirstName"],
                reader["MiddleName"] as string,
                (string)reader["LastName"],
                (string)reader["ContactNumber"],
                (string)reader["Position"],
                reader["LicenseNo"] as string,
                (string)reader["Username"],
                (string)reader["Role"]);
        }

        // Shared by Add and Update (everything except the password columns)
        private static void AddProfileParameters(SqlCommand command, HealthWorker worker)
        {
            command.Parameters.AddWithValue("@FirstName", worker.FirstName);
            command.Parameters.AddWithValue("@MiddleName", (object?)worker.MiddleName ?? DBNull.Value);
            command.Parameters.AddWithValue("@LastName", worker.LastName);
            command.Parameters.AddWithValue("@ContactNumber", worker.ContactNumber);
            command.Parameters.AddWithValue("@Position", worker.Position);
            command.Parameters.AddWithValue("@LicenseNo", (object?)worker.LicenseNo ?? DBNull.Value);
            command.Parameters.AddWithValue("@Username", worker.Username);
            command.Parameters.AddWithValue("@Role", worker.Role);
        }
    }
}