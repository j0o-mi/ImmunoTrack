// Folder: Services/UserSession.cs
using ImmunoTrack.Models;

namespace ImmunoTrack.Services
{
    /// <summary>
    /// Remembers who is logged in while the program runs. Any form can ask
    /// UserSession.CurrentUser (for example, the vaccination form will record
    /// the logged-in health worker as the one who gave the dose).
    /// "static" means there is exactly one session for the whole application.
    /// </summary>
    public static class UserSession
    {
        public static HealthWorker? CurrentUser { get; private set; }

        public static bool IsLoggedIn => CurrentUser != null;

        public static void Start(HealthWorker worker)
        {
            CurrentUser = worker;
        }

        public static void End()
        {
            CurrentUser = null;
        }
    }
}
