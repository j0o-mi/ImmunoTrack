// Folder: Forms/MainForm.cs   (UPDATED: dashboard counts)
using System;
using System.Drawing;
using System.Windows.Forms;
using ImmunoTrack.Models;
using ImmunoTrack.Services;

namespace ImmunoTrack.Forms
{
    /// <summary>
    /// The main window shown after login: a side menu that opens each module and a
    /// dashboard of counts. To add a module later, add ONE line to the constructor.
    /// Only modules that already work have a button (no unfinished buttons).
    /// </summary>
    public partial class MainForm : Form
    {
        private static readonly Color MenuColor = Color.FromArgb(0, 77, 64);
        private static readonly Color MenuHoverColor = Color.FromArgb(0, 105, 92);

        private readonly DefaulterService _defaulterService = new DefaulterService();
        private readonly VaccinationService _vaccinationService = new VaccinationService();

        // Program.cs reads this to know whether to show the login screen again.
        public bool LogoutRequested { get; private set; } = false;

        public MainForm()
        {
            InitializeComponent();

            // ----- THE MENU: one line per module -----
            AddMenuButton("Vaccination", () => new VaccinationForm());
            AddMenuButton("Defaulters", () => new DefaultersForm());
            AddMenuButton("Children", () => new ChildrenForm());
            AddMenuButton("Guardians", () => new GuardiansForm());
            if (UserSession.CurrentUser != null && UserSession.CurrentUser.IsAdmin)
                AddMenuButton("Health Workers", () => new HealthWorkersForm());
            AddMenuButton("Vaccines and Doses", () => new VaccinesForm());

            Load += MainForm_Load;
            btnLogout.Click += btnLogout_Click;
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            HealthWorker? user = UserSession.CurrentUser;
            if (user != null)
            {
                lblWelcome.Text = "Welcome, " + user.FirstName + "!";
                lblRole.Text = "Logged in as " + user.FullName + "  |  " + user.Position;
            }

            LoadDashboard();
        }

        // Fills the dashboard cards from the database.
        private void LoadDashboard()
        {
            try
            {
                StatusCounts counts = _defaulterService.GetStatusCounts();

                lblChildrenCount.Text = counts.Total.ToString();
                lblVaccinationsCount.Text = _vaccinationService.GetTotalVaccinationCount().ToString();
                lblOverdueCount.Text = counts.Overdue.ToString();
                lblDueTodayCount.Text = counts.DueToday.ToString();
                lblUpcomingCount.Text = counts.Upcoming.ToString();
                lblFullyCount.Text = counts.FullyVaccinated.ToString();

                lblHint.Text = "Overview as of " + DateTime.Today.ToString("MMMM d, yyyy");
            }
            catch (Exception ex)
            {
                lblHint.Text = "The overview could not be loaded: " + ex.Message;
            }
        }

        private void btnLogout_Click(object? sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("Log out of IMMUNOTRACK?", "Log out",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (answer != DialogResult.Yes)
                return;

            UserSession.End();
            LogoutRequested = true;
            Close();
        }

        // Creates one styled menu button. "createModule" is a small function that
        // builds the form when the button is clicked (a fresh form each time).
        private void AddMenuButton(string text, Func<Form> createModule)
        {
            Button button = new Button();
            button.BackColor = MenuColor;
            button.Cursor = Cursors.Hand;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = MenuHoverColor;
            button.Font = new Font("Segoe UI", 11F);
            button.ForeColor = Color.White;
            button.Margin = new Padding(0, 0, 0, 2);
            button.Padding = new Padding(20, 0, 0, 0);
            button.Size = new Size(230, 50);
            button.Text = text;
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.UseVisualStyleBackColor = false;

            button.Click += (sender, e) => OpenModule(createModule());

            pnlMenu.Controls.Add(button);
        }

        // Opens a module on top of the main form. When it is closed, the dashboard
        // refreshes, so new records show up in the counts immediately.
        private void OpenModule(Form module)
        {
            using (module)
            {
                module.ShowDialog(this);
            }

            LoadDashboard();
        }
    }
}