// Folder: Models/ChildStatusInfo.cs
using System;

namespace ImmunoTrack.Models
{
    /// <summary>
    /// One row of the defaulter report: a child, their guardian's contact details,
    /// their overall vaccination status, and their most urgent next dose.
    /// </summary>
    public class ChildStatusInfo
    {
        public ChildStatusInfo(Child child, string guardianName, string guardianContact,
                               VaccinationStatus overallStatus, NextDoseInfo? nextDose,
                               int overdueDoseCount)
        {
            Child = child;
            GuardianName = guardianName;
            GuardianContact = guardianContact;
            OverallStatus = overallStatus;
            NextDose = nextDose;
            OverdueDoseCount = overdueDoseCount;
        }

        public Child Child { get; }
        public string GuardianName { get; }
        public string GuardianContact { get; }
        public VaccinationStatus OverallStatus { get; }

        // The most urgent next dose (earliest due date); null if fully vaccinated.
        public NextDoseInfo? NextDose { get; }

        // How many required doses are overdue right now.
        public int OverdueDoseCount { get; }

        // Days the most urgent dose is late (0 if nothing is overdue).
        public int DaysOverdue
        {
            get
            {
                if (NextDose != null && NextDose.Status == VaccinationStatus.Overdue)
                    return -NextDose.DaysFromToday;
                return 0;
            }
        }
    }
}
