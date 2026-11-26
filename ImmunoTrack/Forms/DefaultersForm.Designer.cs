// Folder: Forms/DefaultersForm.Designer.cs
// (This is the layout file.)
#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace ImmunoTrack.Forms
{
    partial class DefaultersForm
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
            lblTitle = new Label();
            pnlTotal = new Panel();
            lblTotalCount = new Label();
            lblTotalCaption = new Label();
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
            lblFilter = new Label();
            cboStatus = new ComboBox();
            lblSearch = new Label();
            txtSearch = new TextBox();
            btnRefresh = new Button();
            lblCount = new Label();
            dgvChildren = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvChildren).BeginInit();
            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 105, 92);
            lblTitle.Location = new Point(20, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Defaulter Monitoring";

            // Summary cards
            BuildCard(pnlTotal, lblTotalCount, lblTotalCaption, "pnlTotal", "lblTotalCount", "lblTotalCaption",
                      "Total Children", Color.FromArgb(55, 71, 79), new Point(20, 55));
            BuildCard(pnlOverdue, lblOverdueCount, lblOverdueCaption, "pnlOverdue", "lblOverdueCount", "lblOverdueCaption",
                      "Overdue (Defaulters)", Color.FromArgb(198, 40, 40), new Point(255, 55));
            BuildCard(pnlDueToday, lblDueTodayCount, lblDueTodayCaption, "pnlDueToday", "lblDueTodayCount", "lblDueTodayCaption",
                      "Due Today", Color.FromArgb(230, 81, 0), new Point(490, 55));
            BuildCard(pnlUpcoming, lblUpcomingCount, lblUpcomingCaption, "pnlUpcoming", "lblUpcomingCount", "lblUpcomingCaption",
                      "Upcoming", Color.FromArgb(0, 121, 107), new Point(725, 55));
            BuildCard(pnlFully, lblFullyCount, lblFullyCaption, "pnlFully", "lblFullyCount", "lblFullyCaption",
                      "Fully Vaccinated", Color.FromArgb(27, 94, 32), new Point(960, 55));

            // Filter row
            lblFilter.AutoSize = true;
            lblFilter.Location = new Point(20, 163);
            lblFilter.Name = "lblFilter";
            lblFilter.Text = "Show:";

            cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatus.Location = new Point(70, 159);
            cboStatus.Name = "cboStatus";
            cboStatus.Size = new Size(230, 25);
            cboStatus.TabIndex = 0;

            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(325, 163);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Child or guardian name:";

            txtSearch.Location = new Point(500, 159);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(250, 25);
            txtSearch.TabIndex = 1;

            btnRefresh.BackColor = Color.FromArgb(0, 121, 107);
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(765, 156);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(110, 32);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;

            lblCount.AutoSize = true;
            lblCount.ForeColor = Color.FromArgb(96, 125, 139);
            lblCount.Location = new Point(895, 163);
            lblCount.Name = "lblCount";
            lblCount.Text = "";

            // dgvChildren
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.FromArgb(0, 105, 92);
            headerStyle.ForeColor = Color.White;
            headerStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            headerStyle.SelectionBackColor = Color.FromArgb(0, 105, 92);
            headerStyle.SelectionForeColor = Color.White;

            DataGridViewCellStyle alternateStyle = new DataGridViewCellStyle();
            alternateStyle.BackColor = Color.FromArgb(237, 247, 245);

            dgvChildren.AllowUserToAddRows = false;
            dgvChildren.AllowUserToDeleteRows = false;
            dgvChildren.AlternatingRowsDefaultCellStyle = alternateStyle;
            dgvChildren.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvChildren.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChildren.BackgroundColor = Color.White;
            dgvChildren.BorderStyle = BorderStyle.None;
            dgvChildren.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvChildren.ColumnHeadersHeight = 34;
            dgvChildren.EnableHeadersVisualStyles = false;
            dgvChildren.Location = new Point(20, 200);
            dgvChildren.MultiSelect = false;
            dgvChildren.Name = "dgvChildren";
            dgvChildren.ReadOnly = true;
            dgvChildren.RowHeadersVisible = false;
            dgvChildren.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChildren.Size = new Size(1160, 500);
            dgvChildren.TabIndex = 3;

            // DefaultersForm
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1200, 720);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1216, 759);
            Name = "DefaultersForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Defaulter Monitoring - IMMUNOTRACK";
            Controls.Add(lblTitle);
            Controls.Add(pnlTotal);
            Controls.Add(pnlOverdue);
            Controls.Add(pnlDueToday);
            Controls.Add(pnlUpcoming);
            Controls.Add(pnlFully);
            Controls.Add(lblFilter);
            Controls.Add(cboStatus);
            Controls.Add(lblSearch);
            Controls.Add(txtSearch);
            Controls.Add(btnRefresh);
            Controls.Add(lblCount);
            Controls.Add(dgvChildren);

            ((System.ComponentModel.ISupportInitialize)dgvChildren).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        // Builds one summary card: a white panel with a big number and a caption.
        private static void BuildCard(Panel panel, Label number, Label caption,
                                      string panelName, string numberName, string captionName,
                                      string captionText, Color color, Point location)
        {
            panel.BackColor = Color.White;
            panel.Location = location;
            panel.Name = panelName;
            panel.Size = new Size(220, 80);
            panel.Controls.Add(number);
            panel.Controls.Add(caption);

            number.AutoSize = true;
            number.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            number.ForeColor = color;
            number.Location = new Point(14, 6);
            number.Name = numberName;
            number.Text = "0";

            caption.AutoSize = true;
            caption.Font = new Font("Segoe UI", 10F);
            caption.ForeColor = Color.FromArgb(96, 125, 139);
            caption.Location = new Point(16, 52);
            caption.Name = captionName;
            caption.Text = captionText;
        }

        #endregion

        private Label lblTitle;
        private Panel pnlTotal;
        private Label lblTotalCount;
        private Label lblTotalCaption;
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
        private Label lblFilter;
        private ComboBox cboStatus;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnRefresh;
        private Label lblCount;
        private DataGridView dgvChildren;
    }
}