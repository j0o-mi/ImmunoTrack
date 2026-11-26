// Folder: Forms/MainForm.Designer.cs   (UPDATED: dashboard cards on the main window)
// (This is the layout file.)
#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace ImmunoTrack.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlSidebar = new Panel();
            lblAppName = new Label();
            lblAppSubtitle = new Label();
            pnlMenu = new FlowLayoutPanel();
            btnLogout = new Button();
            pnlContent = new Panel();
            lblWelcome = new Label();
            lblRole = new Label();
            lblHint = new Label();
            pnlChildren = new Panel();
            lblChildrenCount = new Label();
            lblChildrenCaption = new Label();
            pnlVaccinations = new Panel();
            lblVaccinationsCount = new Label();
            lblVaccinationsCaption = new Label();
            pnlOverdue = new Panel();
            lblOverdueCount = new Label();
            lblOverdueCaption = new Label();
            pnlDueToday = new Panel();
            lblDueTodayCount = new Label();
            lblDueTodayCaption = new Label();
            pnlUpcoming = new Panel();
            lblUpcomingCount = new Label();
            lblUpcomingCaption = new Label();
            pnlFully = new Panel();
            lblFullyCount = new Label();
            lblFullyCaption = new Label();
            pnlSidebar.SuspendLayout();
            pnlContent.SuspendLayout();
            SuspendLayout();

            // pnlSidebar (docked to the left)
            pnlSidebar.BackColor = Color.FromArgb(0, 77, 64);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(230, 650);
            pnlSidebar.Controls.Add(lblAppName);
            pnlSidebar.Controls.Add(lblAppSubtitle);
            pnlSidebar.Controls.Add(pnlMenu);
            pnlSidebar.Controls.Add(btnLogout);

            lblAppName.AutoSize = true;
            lblAppName.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblAppName.ForeColor = Color.White;
            lblAppName.Location = new Point(18, 24);
            lblAppName.Name = "lblAppName";
            lblAppName.Text = "IMMUNOTRACK";

            lblAppSubtitle.AutoSize = true;
            lblAppSubtitle.Font = new Font("Segoe UI", 9F);
            lblAppSubtitle.ForeColor = Color.FromArgb(178, 223, 219);
            lblAppSubtitle.Location = new Point(21, 62);
            lblAppSubtitle.Name = "lblAppSubtitle";
            lblAppSubtitle.Text = "Immunization Tracker";

            // pnlMenu: the menu buttons are added to this panel by MainForm.cs
            pnlMenu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pnlMenu.BackColor = Color.Transparent;
            pnlMenu.FlowDirection = FlowDirection.TopDown;
            pnlMenu.Location = new Point(0, 110);
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Size = new Size(230, 470);
            pnlMenu.WrapContents = false;

            // btnLogout (stays at the bottom of the sidebar)
            btnLogout.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLogout.BackColor = Color.FromArgb(183, 28, 28);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(211, 47, 47);
            btnLogout.Font = new Font("Segoe UI", 11F);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(0, 590);
            btnLogout.Name = "btnLogout";
            btnLogout.Padding = new Padding(20, 0, 0, 0);
            btnLogout.Size = new Size(230, 50);
            btnLogout.TabIndex = 1;
            btnLogout.Text = "Log Out";
            btnLogout.TextAlign = ContentAlignment.MiddleLeft;
            btnLogout.UseVisualStyleBackColor = false;

            // pnlContent (fills the rest of the form)
            pnlContent.BackColor = Color.FromArgb(245, 247, 250);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Name = "pnlContent";
            pnlContent.Controls.Add(lblWelcome);
            pnlContent.Controls.Add(lblRole);
            pnlContent.Controls.Add(lblHint);
            pnlContent.Controls.Add(pnlChildren);
            pnlContent.Controls.Add(pnlVaccinations);
            pnlContent.Controls.Add(pnlOverdue);
            pnlContent.Controls.Add(pnlDueToday);
            pnlContent.Controls.Add(pnlUpcoming);
            pnlContent.Controls.Add(pnlFully);

            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(0, 105, 92);
            lblWelcome.Location = new Point(40, 40);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Text = "Welcome";

            lblRole.AutoSize = true;
            lblRole.Font = new Font("Segoe UI", 11F);
            lblRole.ForeColor = Color.FromArgb(96, 125, 139);
            lblRole.Location = new Point(44, 98);
            lblRole.Name = "lblRole";
            lblRole.Text = "";

            lblHint.AutoSize = true;
            lblHint.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblHint.ForeColor = Color.FromArgb(55, 71, 79);
            lblHint.Location = new Point(44, 160);
            lblHint.Name = "lblHint";
            lblHint.Text = "Overview";

            // Dashboard cards: 3 columns x 2 rows
            BuildCard(pnlChildren, lblChildrenCount, lblChildrenCaption, "pnlChildren", "lblChildrenCount", "lblChildrenCaption",
                      "Registered Children", Color.FromArgb(55, 71, 79), new Point(40, 200));
            BuildCard(pnlVaccinations, lblVaccinationsCount, lblVaccinationsCaption, "pnlVaccinations", "lblVaccinationsCount", "lblVaccinationsCaption",
                      "Vaccinations Recorded", Color.FromArgb(0, 105, 92), new Point(280, 200));
            BuildCard(pnlOverdue, lblOverdueCount, lblOverdueCaption, "pnlOverdue", "lblOverdueCount", "lblOverdueCaption",
                      "Children Overdue", Color.FromArgb(198, 40, 40), new Point(520, 200));
            BuildCard(pnlDueToday, lblDueTodayCount, lblDueTodayCaption, "pnlDueToday", "lblDueTodayCount", "lblDueTodayCaption",
                      "Due Today", Color.FromArgb(230, 81, 0), new Point(40, 305));
            BuildCard(pnlUpcoming, lblUpcomingCount, lblUpcomingCaption, "pnlUpcoming", "lblUpcomingCount", "lblUpcomingCaption",
                      "Upcoming Vaccinations", Color.FromArgb(0, 121, 107), new Point(280, 305));
            BuildCard(pnlFully, lblFullyCount, lblFullyCaption, "pnlFully", "lblFullyCount", "lblFullyCaption",
                      "Fully Vaccinated", Color.FromArgb(27, 94, 32), new Point(520, 305));

            // MainForm. The Fill panel is added first, then the Left panel.
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1000, 650);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1016, 689);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "IMMUNOTRACK - Barangay Immunization and Defaulter Tracker";
            Controls.Add(pnlContent);
            Controls.Add(pnlSidebar);

            pnlSidebar.ResumeLayout(false);
            pnlSidebar.PerformLayout();
            pnlContent.ResumeLayout(false);
            pnlContent.PerformLayout();
            ResumeLayout(false);
        }

        // Builds one dashboard card: a white panel with a big number and a caption.
        private static void BuildCard(Panel panel, Label number, Label caption,
                                      string panelName, string numberName, string captionName,
                                      string captionText, Color color, Point location)
        {
            panel.BackColor = Color.White;
            panel.Location = location;
            panel.Name = panelName;
            panel.Size = new Size(220, 90);
            panel.Controls.Add(number);
            panel.Controls.Add(caption);

            number.AutoSize = true;
            number.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            number.ForeColor = color;
            number.Location = new Point(14, 6);
            number.Name = numberName;
            number.Text = "0";

            caption.AutoSize = true;
            caption.Font = new Font("Segoe UI", 10F);
            caption.ForeColor = Color.FromArgb(96, 125, 139);
            caption.Location = new Point(16, 58);
            caption.Name = captionName;
            caption.Text = captionText;
        }

        #endregion

        private Panel pnlSidebar;
        private Label lblAppName;
        private Label lblAppSubtitle;
        private FlowLayoutPanel pnlMenu;
        private Button btnLogout;
        private Panel pnlContent;
        private Label lblWelcome;
        private Label lblRole;
        private Label lblHint;
        private Panel pnlChildren;
        private Label lblChildrenCount;
        private Label lblChildrenCaption;
        private Panel pnlVaccinations;
        private Label lblVaccinationsCount;
        private Label lblVaccinationsCaption;
        private Panel pnlOverdue;
        private Label lblOverdueCount;
        private Label lblOverdueCaption;
        private Panel pnlDueToday;
        private Label lblDueTodayCount;
        private Label lblDueTodayCaption;
        private Panel pnlUpcoming;
        private Label lblUpcomingCount;
        private Label lblUpcomingCaption;
        private Panel pnlFully;
        private Label lblFullyCount;
        private Label lblFullyCaption;
    }
}