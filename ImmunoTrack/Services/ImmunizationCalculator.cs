// Folder: Services/ImmunizationCalculator.cs
using System;
using System.Collections.Generic;
using System.Linq;
using ImmunoTrack.Models;

namespace ImmunoTrack.Services
{
    /// <summary>
    /// THE due-date rules, in one place. It has no database code: you give it the
    /// child, the configured doses and the child's history, and it answers.
    /// Both the vaccination screen and the defaulter list use it, so the rules
    /// can never differ between screens.
    ///
    /// Rules:
    ///  1. The NEXT dose of a vaccine is its lowest-numbered active dose that has not been recorded.
    ///  2. Its due date is the LATER of:
    ///       a) date of birth + RecommendedAgeDays
    ///       b) previous dose's date + MinimumIntervalDays (only if an interval is configured)
    ///  3. Due date before today = Overdue, today = Due Today, after today = Upcoming.
    ///  4. No next dose in any vaccine = Fully Vaccinated.
    /// These are system calculation rules over configurable data, not medical advice.
    /// </summary>
    public static class ImmunizationCalculator
    {
        // Builds the full picture of one child. childHistory must contain ONLY this child's records.
        public static ChildImmunizationSummary BuildSummary(Child child,
                                                            List<ScheduledDose> activeDoses,
                                                            List<VaccinationHistoryItem> childHistory)
        {
            List<NextDoseInfo> nextDoses = CalculateNextDoses(child, activeDoses, childHistory);
            return new ChildImmunizationSummary(child, childHistory, nextDoses);
        }

        // One next dose per vaccine, sorted with the earliest due date first.
        public static List<NextDoseInfo> CalculateNextDoses(Child child,
                                                            List<ScheduledDose> activeDoses,
                                                            List<VaccinationHistoryItem> childHistory)
        {
            var nextDoses = new List<NextDoseInfo>();
            HashSet<int> recordedScheduleIds = childHistory.Select(h => h.ScheduleID).ToHashSet();

            foreach (IGrouping<int, ScheduledDose> vaccineDoses in activeDoses.GroupBy(d => d.VaccineID))
            {
                // Rule 1: lowest dose number not yet recorded
                ScheduledDose? next = vaccineDoses
                    .Where(d => !recordedScheduleIds.Contains(d.ScheduleID))
                    .OrderBy(d => d.DoseNumber)
                    .FirstOrDefault();

                if (next == null)
                    continue;   // every dose of this vaccine is already given

                DateTime dueDate = CalculateDueDate(child, next, childHistory);
                nextDoses.Add(new NextDoseInfo(next.ScheduleID, next.VaccineID, next.VaccineName,
                                               next.DoseNumber, dueDate));
            }

            return nextDoses
                .OrderBy(n => n.DueDate)
                .ThenBy(n => n.VaccineName)
                .ToList();
        }

        // Rule 2: the later of the age-based date and the interval-based date.
        public static DateTime CalculateDueDate(Child child, ScheduledDose dose,
                                                List<VaccinationHistoryItem> childHistory)
        {
            DateTime byAge = child.DateOfBirth.Date.AddDays(dose.RecommendedAgeDays);

            if (!dose.MinimumIntervalDays.HasValue)
                return byAge;

            VaccinationHistoryItem? previous = FindPreviousRecord(childHistory, dose.VaccineID, dose.DoseNumber);
            if (previous == null)
                return byAge;

            DateTime byInterval = previous.DateAdministered.Date.AddDays(dose.MinimumIntervalDays.Value);
            return byInterval > byAge ? byInterval : byAge;
        }

        // The most recent earlier dose of the same vaccine that was given (null for dose 1).
        public static VaccinationHistoryItem? FindPreviousRecord(List<VaccinationHistoryItem> childHistory,
                                                                 int vaccineId, int doseNumber)
        {
            return childHistory
                .Where(h => h.VaccineID == vaccineId && h.DoseNumber < doseNumber)
                .OrderByDescending(h => h.DoseNumber)
                .FirstOrDefault();
        }
    }
}
