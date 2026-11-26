// Folder: Models/StatusCounts.cs
namespace ImmunoTrack.Models
{
    /// <summary>How many children are in each vaccination status (for the summary cards and dashboard).</summary>
    public class StatusCounts
    {
        public StatusCounts(int total, int overdue, int dueToday, int upcoming, int fullyVaccinated)
        {
            Total = total;
            Overdue = overdue;
            DueToday = dueToday;
            Upcoming = upcoming;
            FullyVaccinated = fullyVaccinated;
        }

        public int Total { get; }
        public int Overdue { get; }
        public int DueToday { get; }
        public int Upcoming { get; }
        public int FullyVaccinated { get; }
    }
}
