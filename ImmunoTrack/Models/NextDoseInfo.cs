// Folder: Models/NextDoseInfo.cs
using System;

namespace ImmunoTrack.Models
{
    /// <summary>
    /// The next required dose of one vaccine for one child, with its due date.
    /// The status is NOT stored: it is worked out from the due date and today's
    /// date every time, so it can never be out of date.
    /// </summary>
    public class NextDoseInfo
    {
        public NextDoseInfo(int scheduleId, int vaccineId, string vaccineName, int doseNumber, DateTime dueDate)
        {
            ScheduleID = scheduleId;
            VaccineID = vaccineId;
            VaccineName = vaccineName;
            DoseNumber = doseNumber;
            DueDate = dueDate.Date;
        }

        public int ScheduleID { get; }
        public int VaccineID { get; }
        public string VaccineName { get; }
        public int DoseNumber { get; }
        public DateTime DueDate { get; }

        // Negative = days overdue, 0 = today, positive = days until due.
        public int DaysFromToday => (DueDate - DateTime.Today).Days;

        public VaccinationStatus Status
        {
            get
            {
                int days = DaysFromToday;
                if (days < 0)
                    return VaccinationStatus.Overdue;
                if (days == 0)
                    return VaccinationStatus.DueToday;
                return VaccinationStatus.Upcoming;
            }
        }

        // e.g. "Overdue (125 days)", "Due Today", "Upcoming (in 15 days)"
        public string GetStatusText()
        {
            int days = DaysFromToday;

            switch (Status)
            {
                case VaccinationStatus.Overdue:
                    return "Overdue (" + Plural(-days) + ")";
                case VaccinationStatus.DueToday:
                    return "Due Today";
                default:
                    return "Upcoming (in " + Plural(days) + ")";
            }
        }

        private static string Plural(int days)
        {
            return days == 1 ? "1 day" : days + " days";
        }
    }
}
