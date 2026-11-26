// Folder: Models/VaccinationStatus.cs
namespace ImmunoTrack.Models
{
    /// <summary>The four vaccination statuses used across the system.</summary>
    public enum VaccinationStatus
    {
        Upcoming,          // the next due date is in the future
        DueToday,          // the next dose is due today
        Overdue,           // the due date has passed and the dose is not recorded
        FullyVaccinated    // every active dose has been recorded
    }

    public static class VaccinationStatusExtensions
    {
        // Text shown on screen (e.g. "Due Today" instead of "DueToday").
        public static string ToDisplayText(this VaccinationStatus status)
        {
            switch (status)
            {
                case VaccinationStatus.DueToday:
                    return "Due Today";
                case VaccinationStatus.FullyVaccinated:
                    return "Fully Vaccinated";
                default:
                    return status.ToString();
            }
        }
    }
}
