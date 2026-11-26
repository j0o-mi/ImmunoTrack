// Folder: DataAccess/ChildDataAccess.cs
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ImmunoTrack.Models;

namespace ImmunoTrack.DataAccess
{
    /// <summary>
    /// Talks to the Children table. Same pattern as GuardianDataAccess:
    /// parameterized SQL, no validation, no message boxes.
    /// </summary>
    public class ChildDataAccess
    {
        // Every query starts with this, so the column list exists in one place only.
        private const string SelectFrom =
            @"SELECT c.ChildID, c.FirstName, c.MiddleName, c.LastName, c.DateOfBirth,
                     c.Sex, c.BloodType, c.Address, c.GuardianID
              FROM Children c ";

        // READ: all children
        public List<Child> GetAll()
        {
            return ReadChildren(SelectFrom + "ORDER BY c.LastName, c.FirstName", null);
        }

        // READ: one child by ID (null if not found)
        public Child? GetById(int childId)
        {
            var parameters = new List<SqlParameter> { new SqlParameter("@ChildID", childId) };
            List<Child> result = ReadChildren(SelectFrom + "WHERE c.ChildID = @ChildID", parameters);
            return result.Count > 0 ? result[0] : null;
        }

        // SEARCH: by the child's name (partial match)
        public List<Child> SearchByChildName(string name)
        {
            string sql = SelectFrom +
                @"WHERE (c.FirstName + ' ' + c.LastName) LIKE @Search
                     OR (c.FirstName + ' ' + ISNULL(c.MiddleName + ' ', '') + c.LastName) LIKE @Search
                  ORDER BY c.LastName, c.FirstName";
            var parameters = new List<SqlParameter> { new SqlParameter("@Search", "%" + name + "%") };
            return ReadChildren(sql, parameters);
        }

        // SEARCH: by the guardian's name (partial match)
        public List<Child> SearchByGuardianName(string name)
        {
            string sql = SelectFrom +
                @"INNER JOIN Guardians g ON g.GuardianID = c.GuardianID
                  WHERE (g.FirstName + ' ' + g.LastName) LIKE @Search
                     OR (g.FirstName + ' ' + ISNULL(g.MiddleName + ' ', '') + g.LastName) LIKE @Search
                  ORDER BY c.LastName, c.FirstName";
            var parameters = new List<SqlParameter> { new SqlParameter("@Search", "%" + name + "%") };
            return ReadChildren(sql, parameters);
        }

        // Is there already a child with this name and birth date? (excludeChildId = the child being edited, or 0)
        public bool ExistsDuplicate(string firstName, string lastName, DateTime dateOfBirth, int excludeChildId)
        {
            const string sql = @"SELECT COUNT(*) FROM Children
                                 WHERE FirstName = @FirstName AND LastName = @LastName
                                   AND DateOfBirth = @DateOfBirth AND ChildID <> @ExcludeID";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth.Date);
                command.Parameters.AddWithValue("@ExcludeID", excludeChildId);
                connection.Open();
                return (int)command.ExecuteScalar() > 0;
            }
        }

        // CREATE: returns the new ChildID
        public int Add(Child child)
        {
            const string sql = @"INSERT INTO Children
                                    (FirstName, MiddleName, LastName, DateOfBirth, Sex, BloodType, Address, GuardianID)
                                 VALUES
                                    (@FirstName, @MiddleName, @LastName, @DateOfBirth, @Sex, @BloodType, @Address, @GuardianID);
                                 SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameters(command, child);
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        // UPDATE
        public void Update(Child child)
        {
            const string sql = @"UPDATE Children
                                 SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName,
                                     DateOfBirth = @DateOfBirth, Sex = @Sex, BloodType = @BloodType,
                                     Address = @Address, GuardianID = @GuardianID
                                 WHERE ChildID = @ChildID";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddParameters(command, child);
                command.Parameters.AddWithValue("@ChildID", child.ChildID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // DELETE (the database refuses with error 547 if the child has vaccination records)
        public void Delete(int childId)
        {
            const string sql = "DELETE FROM Children WHERE ChildID = @ChildID";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ChildID", childId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // ---------- private helpers ----------

        private List<Child> ReadChildren(string sql, List<SqlParameter>? parameters)
        {
            var children = new List<Child>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters.ToArray());

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        children.Add(MapChild(reader));
                }
            }

            return children;
        }

        // One database row -> one Child object
        private static Child MapChild(SqlDataReader reader)
        {
            return new Child(
                (int)reader["ChildID"],
                (string)reader["FirstName"],
                reader["MiddleName"] as string,
                (string)reader["LastName"],
                (DateTime)reader["DateOfBirth"],
                (string)reader["Sex"],
                reader["BloodType"] as string,
                (string)reader["Address"],
                (int)reader["GuardianID"]);
        }

        private static void AddParameters(SqlCommand command, Child child)
        {
            command.Parameters.AddWithValue("@FirstName", child.FirstName);
            command.Parameters.AddWithValue("@MiddleName", (object?)child.MiddleName ?? DBNull.Value);
            command.Parameters.AddWithValue("@LastName", child.LastName);
            command.Parameters.AddWithValue("@DateOfBirth", child.DateOfBirth.Date);
            command.Parameters.AddWithValue("@Sex", child.Sex);
            command.Parameters.AddWithValue("@BloodType", (object?)child.BloodType ?? DBNull.Value);
            command.Parameters.AddWithValue("@Address", child.Address);
            command.Parameters.AddWithValue("@GuardianID", child.GuardianID);
        }
    }
}
