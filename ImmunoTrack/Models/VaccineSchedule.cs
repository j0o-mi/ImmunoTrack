// Folder: Models/VaccineSchedule.cs
using System;

namespace ImmunoTrack.Models
{
    /// <summary>
    /// One dose of one vaccine, with when it is due:
    ///   RecommendedAgeDays  = the child's age (in days) when the dose is due
    ///   MinimumIntervalDays = minimum days after the PREVIOUS dose (blank for dose 1)
    /// These values are configurable data, not hard-coded medical rules.
    /// </summary>
    public class VaccineSchedule
    {
        private int _doseNumber = 1;
        private int _recommendedAgeDays;
        private int? _minimumIntervalDays;

        public VaccineSchedule()
        {
        }

        public VaccineSchedule(int scheduleId, int vaccineId, int doseNumber,
                               int recommendedAgeDays, int? minimumIntervalDays, bool isActive)
        {
            ScheduleID = scheduleId;
            VaccineID = vaccineId;
            DoseNumber = doseNumber;
            RecommendedAgeDays = recommendedAgeDays;
            MinimumIntervalDays = minimumIntervalDays;
            IsActive = isActive;
        }

        public int ScheduleID { get; set; }

        public int VaccineID { get; set; }

        public int DoseNumber
        {
            get => _doseNumber;
            set
            {
                if (value < 1)
                    throw new ArgumentException("Dose number must be 1 or higher.");
                _doseNumber = value;
            }
        }

        public int RecommendedAgeDays
        {
            get => _recommendedAgeDays;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Recommended age cannot be negative.");
                _recommendedAgeDays = value;
            }
        }

        public int? MinimumIntervalDays
        {
            get => _minimumIntervalDays;
            set
            {
                if (value.HasValue && value.Value < 0)
                    throw new ArgumentException("Minimum interval cannot be negative.");
                _minimumIntervalDays = value;
            }
        }

        public bool IsActive { get; set; } = true;

        public override string ToString()
        {
            return "Dose " + DoseNumber;
        }
    }
}
