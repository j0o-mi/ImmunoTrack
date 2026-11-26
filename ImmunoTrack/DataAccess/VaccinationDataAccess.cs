// Folder: DataAccess/VaccinationDataAccess.cs
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ImmunoTrack.Models;

namespace ImmunoTrack.DataAccess
{
    /// <summary>
    /// Talks to VaccinationRecords (and reads the schedule). It only fetches and
    /// saves data: the due-date rules live in ImmunizationCalculator.
    /// </summary>
    public class VaccinationDataAccess
    {
        // Every history query starts with this joined SELECT.
        private const string HistorySelect =
            @"SELECT r.RecordID, r.ChildID, r.ScheduleID, s.VaccineID, v.VaccineName, s.DoseNumber,
                     r.DateAdministered, r.HealthWorkerID,
                     hw.FirstName + ' ' + hw.LastName AS AdministeredBy, r.Remarks
              FROM VaccinationRecords r
              INNER JOIN VaccineSchedule s ON s.ScheduleID = r.ScheduleID
              INNER JOIN Vaccines v ON v.VaccineID = s.VaccineID
              INNER JOIN HealthWorkers hw ON hw.HealthWorkerID = r.HealthWorkerID ";

        // All doses that currently count: active schedule rows of active vaccines.
        public List<ScheduledDose> GetActiveScheduledDoses()
        {
            const string sql = @"SELECT s.ScheduleID, s.VaccineID, v.VaccineName, s.DoseNumber,
                                        s.RecommendedAgeDays, s.MinimumIntervalDays
                                 FROM VaccineSchedule s
                                 INNER JOIN Vaccines v ON v.VaccineID = s.VaccineID
                                 WHERE s.IsActive = 1 AND v.IsActive = 1
                                 ORDER BY v.VaccineName, s.DoseNumber";

            var doses = new List<ScheduledDose>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        doses.Add(new ScheduledDose(
                            (int)reader["ScheduleID"],
                            (int)reader["VaccineID"],
                            (string)reader["VaccineName"],
                            (int)reader["DoseNumber"],
                            (int)reader["RecommendedAgeDays"],
                            reader["MinimumIntervalDays"] == DBNull.Value
                                ? (int?)null
                                : (int)reader["MinimumIntervalDays"]));
                    }
                }
            }

            return doses;
        }

        // One child's full history (includes doses of vaccines that are now inactive).
        public List<VaccinationHistoryItem> GetHistoryByChild(int childId)
        {
            string sql = HistorySelect + "WHERE r.ChildID = @ChildID ORDER BY r.DateAdministered, s.DoseNumber";
            var parameters = new List<SqlParameter> { new SqlParameter("@ChildID", childId) };
            return ReadHistory(sql, parameters);
        }

        // Every record of every child (used by the defaulter list).
        public List<VaccinationHistoryItem> GetAllHistory()
        {
            string sql = HistorySelect + "ORDER BY r.ChildID, r.DateAdministered, s.DoseNumber";
            return ReadHistory(sql, null);
        }

        // CREATE: returns the new RecordID
        public int Add(VaccinationRecord record)
        {
            const string sql = @"INSERT INTO VaccinationRecords
                                    (ChildID, ScheduleID, DateAdministered, HealthWorkerID, Remarks)
                                 VALUES
                                    (@ChildID, @ScheduleID, @DateAdministered, @HealthWorkerID, @Remarks);
                                 SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ChildID", record.ChildID);
                command.Parameters.AddWithValue("@ScheduleID", record.ScheduleID);
                command.Parameters.AddWithValue("@DateAdministered", record.DateAdministered.Date);
                command.Parameters.AddWithValue("@HealthWorkerID", record.HealthWorkerID);
                command.Parameters.AddWithValue("@Remarks", (object?)record.Remarks ?? DBNull.Value);
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        // Total number of vaccination records (for the dashboard).
        public int CountRecords()
        {
            const string sql = "SELECT COUNT(*) FROM VaccinationRecords";

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        // ---------- private helper ----------

        private List<VaccinationHistoryItem> ReadHistory(string sql, List<SqlParameter>? parameters)
        {
            var items = new List<VaccinationHistoryItem>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters.ToArray());

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new VaccinationHistoryItem(
                            (int)reader["RecordID"],
                            (int)reader["ChildID"],
                            (int)reader["ScheduleID"],
                            (int)reader["VaccineID"],
                            (string)reader["VaccineName"],
                            (int)reader["DoseNumber"],
                            (DateTime)reader["DateAdministered"],
                            (int)reader["HealthWorkerID"],
                            (string)reader["AdministeredBy"],
                            reader["Remarks"] as string));
                    }
                }
            }

            return items;
        }
    }
}
