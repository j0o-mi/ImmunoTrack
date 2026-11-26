// Folder: Models/VaccinationHistoryItem.cs
using System;

namespace ImmunoTrack.Models
{
    /// <summary>
    /// A vaccination record joined with readable details (vaccine name, dose
    /// number, health worker name). Used for history lists and for the
    /// due-date calculation.
    /// </summary>
    public class VaccinationHistoryItem
    {
        public VaccinationHistoryItem(int recordId, int childId, int scheduleId, int vaccineId,
                                      string vaccineName, int doseNumber, DateTime dateAdministered,
                                      int healthWorkerId, string administeredBy, string? remarks)
        {
            RecordID = recordId;
            ChildID = childId;
            ScheduleID = scheduleId;
            VaccineID = vaccineId;
            VaccineName = vaccineName;
            DoseNumber = doseNumber;
            DateAdministered = dateAdministered;
            HealthWorkerID = healthWorkerId;
            AdministeredBy = administeredBy;
            Remarks = remarks;
        }

        public int RecordID { get; }
        public int ChildID { get; }
        public int ScheduleID { get; }
        public int VaccineID { get; }
        public string VaccineName { get; }
        public int DoseNumber { get; }
        public DateTime DateAdministered { get; }
        public int HealthWorkerID { get; }
        public string AdministeredBy { get; }
        public string? Remarks { get; }
    }
}
