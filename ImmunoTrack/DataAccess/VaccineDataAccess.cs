// Folder: DataAccess/VaccineDataAccess.cs
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ImmunoTrack.Models;

namespace ImmunoTrack.DataAccess
{
    /// <summary>
    /// Talks to the Vaccines and VaccineSchedule tables (two tables that belong together).
    /// Parameterized SQL only; no validation and no message boxes.
    /// </summary>
    public class VaccineDataAccess
    {
        private const string VaccineColumns = "VaccineID, VaccineName, Description, IsActive";
        private const string ScheduleColumns =
            "ScheduleID, VaccineID, DoseNumber, RecommendedAgeDays, MinimumIntervalDays, IsActive";

        // =====================  VACCINES  =====================

        public List<Vaccine> GetAllVaccines()
        {
            var vaccines = new List<Vaccine>();
            string sql = $"SELECT {VaccineColumns} FROM Vaccines ORDER BY VaccineName";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vaccines.Add(new Vaccine(
                            (int)reader["VaccineID"],
                            (string)reader["VaccineName"],
                            reader["Description"] as string,
                            (bool)reader["IsActive"]));
                    }
                }
            }

            return vaccines;
        }

        public bool VaccineNameExists(string vaccineName, int excludeVaccineId)
        {
            const string sql = @"SELECT COUNT(*) FROM Vaccines
                                 WHERE VaccineName = @VaccineName AND VaccineID <> @ExcludeID";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@VaccineName", vaccineName);
                command.Parameters.AddWithValue("@ExcludeID", excludeVaccineId);
                connection.Open();
                return (int)command.ExecuteScalar() > 0;
            }
        }

        public int AddVaccine(Vaccine vaccine)
        {
            const string sql = @"INSERT INTO Vaccines (VaccineName, Description, IsActive)
                                 VALUES (@VaccineName, @Description, @IsActive);
                                 SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddVaccineParameters(command, vaccine);
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public void UpdateVaccine(Vaccine vaccine)
        {
            const string sql = @"UPDATE Vaccines
                                 SET VaccineName = @VaccineName, Description = @Description, IsActive = @IsActive
                                 WHERE VaccineID = @VaccineID";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddVaccineParameters(command, vaccine);
                command.Parameters.AddWithValue("@VaccineID", vaccine.VaccineID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Error 547 if the vaccine still has dose schedules
        public void DeleteVaccine(int vaccineId)
        {
            const string sql = "DELETE FROM Vaccines WHERE VaccineID = @VaccineID";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@VaccineID", vaccineId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // =====================  VACCINE SCHEDULE (DOSES)  =====================

        public List<VaccineSchedule> GetSchedulesByVaccine(int vaccineId)
        {
            var schedules = new List<VaccineSchedule>();
            string sql = $@"SELECT {ScheduleColumns} FROM VaccineSchedule
                            WHERE VaccineID = @VaccineID ORDER BY DoseNumber";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@VaccineID", vaccineId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schedules.Add(new VaccineSchedule(
                            (int)reader["ScheduleID"],
                            (int)reader["VaccineID"],
                            (int)reader["DoseNumber"],
                            (int)reader["RecommendedAgeDays"],
                            reader["MinimumIntervalDays"] == DBNull.Value
                                ? (int?)null
                                : (int)reader["MinimumIntervalDays"],
                            (bool)reader["IsActive"]));
                    }
                }
            }

            return schedules;
        }

        public bool DoseExists(int vaccineId, int doseNumber)
        {
            const string sql = @"SELECT COUNT(*) FROM VaccineSchedule
                                 WHERE VaccineID = @VaccineID AND DoseNumber = @DoseNumber";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@VaccineID", vaccineId);
                command.Parameters.AddWithValue("@DoseNumber", doseNumber);
                connection.Open();
                return (int)command.ExecuteScalar() > 0;
            }
        }

        public int AddSchedule(VaccineSchedule schedule)
        {
            const string sql = @"INSERT INTO VaccineSchedule
                                    (VaccineID, DoseNumber, RecommendedAgeDays, MinimumIntervalDays, IsActive)
                                 VALUES
                                    (@VaccineID, @DoseNumber, @RecommendedAgeDays, @MinimumIntervalDays, @IsActive);
                                 SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                AddScheduleParameters(command, schedule);
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        // The dose number and vaccine are never changed after a dose is created.
        public void UpdateSchedule(VaccineSchedule schedule)
        {
            const string sql = @"UPDATE VaccineSchedule
                                 SET RecommendedAgeDays = @RecommendedAgeDays,
                                     MinimumIntervalDays = @MinimumIntervalDays,
                                     IsActive = @IsActive
                                 WHERE ScheduleID = @ScheduleID";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@RecommendedAgeDays", schedule.RecommendedAgeDays);
                command.Parameters.AddWithValue("@MinimumIntervalDays", (object?)schedule.MinimumIntervalDays ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", schedule.IsActive);
                command.Parameters.AddWithValue("@ScheduleID", schedule.ScheduleID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Error 547 if the dose has already been recorded for any child
        public void DeleteSchedule(int scheduleId)
        {
            const string sql = "DELETE FROM VaccineSchedule WHERE ScheduleID = @ScheduleID";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ScheduleID", scheduleId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // ---------- private helpers ----------

        private static void AddVaccineParameters(SqlCommand command, Vaccine vaccine)
        {
            command.Parameters.AddWithValue("@VaccineName", vaccine.VaccineName);
            command.Parameters.AddWithValue("@Description", (object?)vaccine.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", vaccine.IsActive);
        }

        private static void AddScheduleParameters(SqlCommand command, VaccineSchedule schedule)
        {
            command.Parameters.AddWithValue("@VaccineID", schedule.VaccineID);
            command.Parameters.AddWithValue("@DoseNumber", schedule.DoseNumber);
            command.Parameters.AddWithValue("@RecommendedAgeDays", schedule.RecommendedAgeDays);
            command.Parameters.AddWithValue("@MinimumIntervalDays", (object?)schedule.MinimumIntervalDays ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", schedule.IsActive);
        }
    }
}
