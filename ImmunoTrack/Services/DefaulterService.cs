// Folder: Services/DefaulterService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using ImmunoTrack.DataAccess;
using ImmunoTrack.Models;

namespace ImmunoTrack.Services
{
    /// <summary>
    /// Business layer for defaulter monitoring. A child is a defaulter (Overdue) only
    /// when a REQUIRED next dose has a due date that has passed and the dose is not
    /// recorded. An old vaccination history alone never makes a child a defaulter.
    /// It uses the same ImmunizationCalculator as the Vaccination screen, so both
    /// screens always agree.
    /// </summary>
    public class DefaulterService
    {
        private readonly ChildDataAccess _childDataAccess = new ChildDataAccess();
        private readonly GuardianDataAccess _guardianDataAccess = new GuardianDataAccess();
        private readonly VaccinationDataAccess _vaccinationDataAccess = new VaccinationDataAccess();

        // The status of EVERY child, most urgent first.
        // Data is loaded once (4 queries) and calculated in memory.
        public List<ChildStatusInfo> GetChildStatuses()
        {
            List<Child> children = _childDataAccess.GetAll();
            Dictionary<int, Guardian> guardians = _guardianDataAccess.GetAll().ToDictionary(g => g.GuardianID);
            List<ScheduledDose> activeDoses = _vaccinationDataAccess.GetActiveScheduledDoses();
            Dictionary<int, List<VaccinationHistoryItem>> historyByChild = _vaccinationDataAccess
                .GetAllHistory()
                .GroupBy(h => h.ChildID)
                .ToDictionary(g => g.Key, g => g.ToList());

            var result = new List<ChildStatusInfo>();

            foreach (Child child in children)
            {
                List<VaccinationHistoryItem> history;
                if (!historyByChild.TryGetValue(child.ChildID, out history!))
                    history = new List<VaccinationHistoryItem>();

                ChildImmunizationSummary summary = ImmunizationCalculator.BuildSummary(child, activeDoses, history);

                guardians.TryGetValue(child.GuardianID, out Guardian? guardian);

                result.Add(new ChildStatusInfo(
                    child,
                    guardian == null ? "?" : guardian.FullName,
                    guardian == null ? "-" : guardian.ContactNumber,
                    summary.OverallStatus,
                    summary.NextRequired,
                    summary.NextDoses.Count(n => n.Status == VaccinationStatus.Overdue)));
            }

            return result
                .OrderBy(i => StatusRank(i.OverallStatus))
                .ThenBy(i => i.NextDose != null ? i.NextDose.DueDate : DateTime.MaxValue)
                .ThenBy(i => i.Child.LastName)
                .ToList();
        }

        // Only the defaulters: children with at least one overdue dose.
        public List<ChildStatusInfo> GetDefaulters()
        {
            return GetChildStatuses()
                .Where(i => i.OverallStatus == VaccinationStatus.Overdue)
                .ToList();
        }

        // Filters an already-loaded list by status (null = all) and by child or guardian name.
        public List<ChildStatusInfo> Filter(List<ChildStatusInfo> all, VaccinationStatus? status, string searchText)
        {
            IEnumerable<ChildStatusInfo> query = all;

            if (status.HasValue)
                query = query.Where(i => i.OverallStatus == status.Value);

            string text = (searchText ?? string.Empty).Trim();
            if (text.Length > 0)
            {
                query = query.Where(i =>
                    i.Child.FullName.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                    i.GuardianName.Contains(text, StringComparison.OrdinalIgnoreCase));
            }

            return query.ToList();
        }

        public StatusCounts CountByStatus(List<ChildStatusInfo> all)
        {
            return new StatusCounts(
                all.Count,
                all.Count(i => i.OverallStatus == VaccinationStatus.Overdue),
                all.Count(i => i.OverallStatus == VaccinationStatus.DueToday),
                all.Count(i => i.OverallStatus == VaccinationStatus.Upcoming),
                all.Count(i => i.OverallStatus == VaccinationStatus.FullyVaccinated));
        }

        // Convenience for the dashboard.
        public StatusCounts GetStatusCounts()
        {
            return CountByStatus(GetChildStatuses());
        }

        // Sort order: most urgent status first.
        private static int StatusRank(VaccinationStatus status)
        {
            switch (status)
            {
                case VaccinationStatus.Overdue:
                    return 0;
                case VaccinationStatus.DueToday:
                    return 1;
                case VaccinationStatus.Upcoming:
                    return 2;
                default:
                    return 3;
            }
        }
    }
}
