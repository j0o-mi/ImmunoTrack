// Folder: Models/ChildImmunizationSummary.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImmunoTrack.Models
{
    /// <summary>
    /// Everything about one child's immunization in one object: the history,
    /// the next required doses, and the overall status.
    /// </summary>
    public class ChildImmunizationSummary
    {
        public ChildImmunizationSummary(Child child, List<VaccinationHistoryItem> history,
                                        List<NextDoseInfo> nextDoses)
        {
            Child = child;
            History = history;
            NextDoses = nextDoses;   // already sorted by due date (earliest first)
        }

        public Child Child { get; }
        public List<VaccinationHistoryItem> History { get; }
        public List<NextDoseInfo> NextDoses { get; }

        // The most recently administered dose (null if nothing recorded yet).
        public VaccinationHistoryItem? LatestRecord =>
            History.OrderByDescending(h => h.DateAdministered)
                   .ThenByDescending(h => h.RecordID)
                   .FirstOrDefault();

        // The most urgent next dose (null if every dose is recorded).
        public NextDoseInfo? NextRequired => NextDoses.Count > 0 ? NextDoses[0] : null;

        // The most urgent dose decides the child's overall status.
        public VaccinationStatus OverallStatus =>
            NextRequired == null ? VaccinationStatus.FullyVaccinated : NextRequired.Status;
    }
}
