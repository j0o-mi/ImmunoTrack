// Folder: Models/VaccinationRecord.cs
using System;
using ImmunoTrack.Helpers;

namespace ImmunoTrack.Models
{
    /// <summary>
    /// One vaccination transaction: a specific dose given to a specific child,
    /// on a date, by a health worker. The vaccine and dose number are found
    /// through the ScheduleID.
    /// </summary>
    public class VaccinationRecord
    {
        private int _childId;
        private int _scheduleId;
        private DateTime _dateAdministered;
        private int _healthWorkerId;
        private string? _remarks;

        public VaccinationRecord()
        {
        }

        public VaccinationRecord(int childId, int scheduleId, DateTime dateAdministered,
                                 int healthWorkerId, string? remarks)
        {
            ChildID = childId;
            ScheduleID = scheduleId;
            DateAdministered = dateAdministered;
            HealthWorkerID = healthWorkerId;
            Remarks = remarks;
        }

        public int RecordID { get; set; }

        public int ChildID
        {
            get => _childId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Select a child first.");
                _childId = value;
            }
        }

        public int ScheduleID
        {
            get => _scheduleId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Select the dose to record.");
                _scheduleId = value;
            }
        }

        public DateTime DateAdministered
        {
            get => _dateAdministered;
            set
            {
                if (value.Date > DateTime.Today)
                    throw new ArgumentException("The date administered cannot be in the future.");
                _dateAdministered = value.Date;
            }
        }

        public int HealthWorkerID
        {
            get => _healthWorkerId;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("No health worker is logged in.");
                _healthWorkerId = value;
            }
        }

        public string? Remarks
        {
            get => _remarks;
            set => _remarks = InputValidator.OptionalText(value, "Remarks", 250);
        }
    }
}
