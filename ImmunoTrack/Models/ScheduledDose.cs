// Folder: Models/ScheduledDose.cs
using System;

namespace ImmunoTrack.Models
{
    /// <summary>
    /// One configured dose together with its vaccine's name: the input the
    /// due-date calculation needs (read from VaccineSchedule + Vaccines).
    /// </summary>
    public class ScheduledDose
    {
        public ScheduledDose(int scheduleId, int vaccineId, string vaccineName, int doseNumber,
                             int recommendedAgeDays, int? minimumIntervalDays)
        {
            ScheduleID = scheduleId;
            VaccineID = vaccineId;
            VaccineName = vaccineName;
            DoseNumber = doseNumber;
            RecommendedAgeDays = recommendedAgeDays;
            MinimumIntervalDays = minimumIntervalDays;
        }

        public int ScheduleID { get; }
        public int VaccineID { get; }
        public string VaccineName { get; }
        public int DoseNumber { get; }
        public int RecommendedAgeDays { get; }
        public int? MinimumIntervalDays { get; }
    }
}
