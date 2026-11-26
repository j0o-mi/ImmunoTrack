// File: Program.cs (in the project root, replaces the existing Program.cs)
using System;
using System.Windows.Forms;
using ImmunoTrack.Forms;

namespace ImmunoTrack
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Show the login screen; after a successful login show the main window.
            // Logging out returns to the login screen; closing the window exits the app.
            while (true)
            {
                using (LoginForm login = new LoginForm())
                {
                    if (login.ShowDialog() != DialogResult.OK)
                        return;     // Exit pressed: end the program
                }

                using (MainForm main = new MainForm())
                {
                    main.ShowDialog();

                    if (!main.LogoutRequested)
                        return;     // window closed with X: end the program
                }
            }
        }
    }
}