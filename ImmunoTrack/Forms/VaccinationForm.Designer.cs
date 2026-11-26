// Folder: Forms/VaccinationForm.Designer.cs
// (This is the layout file.)
#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace ImmunoTrack.Forms
{
    partial class VaccinationForm
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
            grpChild = new GroupBox();
            lblFind = new Label();
            txtFindChild = new TextBox();
            lblChild = new Label();
            cboChild = new ComboBox();
            lblChildInfo = new Label();
            grpSummary = new GroupBox();
            lblLatest = new Label();
            lblNext = new Label();
            lblStatus = new Label();
            grpRecord = new GroupBox();
            lblDose = new Label();
            cboDose = new ComboBox();
            lblDate = new Label();
            dtpDate = new DateTimePicker();
            lblWorker = new Label();
            txtWorker = new TextBox();
            lblRemarks = new Label();
            txtRemarks = new TextBox();
            btnRecord = new Button();
            lblNextTitle = new Label();
            dgvNextDoses = new DataGridView();
            lblHistoryTitle = new Label();
            dgvHistory = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvNextDoses).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvHistory).BeginInit();
            grpChild.SuspendLayout();
            grpSummary.SuspendLayout();
            grpRecord.SuspendLayout();
            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 105, 92);
            lblTitle.Location = new Point(20, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Vaccination";

            // ===== grpChild =====
            grpChild.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpChild.Location = new Point(20, 55);
            grpChild.Name = "grpChild";
            grpChild.Size = new Size(1060, 85);
            grpChild.TabStop = false;
            grpChild.Text = "Select Child";
            grpChild.Controls.Add(lblFind);
            grpChild.Controls.Add(txtFindChild);
            grpChild.Controls.Add(lblChild);
            grpChild.Controls.Add(cboChild);
            grpChild.Controls.Add(lblChildInfo);

            lblFind.AutoSize = true;
            lblFind.Location = new Point(15, 25);
            lblFind.Name = "lblFind";
            lblFind.Text = "Find (name or ID)";
            txtFindChild.Location = new Point(15, 47);
            txtFindChild.Name = "txtFindChild";
            txtFindChild.Size = new Size(250, 25);
            txtFindChild.TabIndex = 0;

            lblChild.AutoSize = true;
            lblChild.Location = new Point(285, 25);
            lblChild.Name = "lblChild";
            lblChild.Text = "Child *";
            cboChild.DropDownStyle = ComboBoxStyle.DropDownList;
            cboChild.Location = new Point(285, 47);
            cboChild.Name = "cboChild";
            cboChild.Size = new Size(400, 25);
            cboChild.TabIndex = 1;

            lblChildInfo.AutoSize = false;
            lblChildInfo.ForeColor = Color.FromArgb(55, 71, 79);
            lblChildInfo.Location = new Point(705, 28);
            lblChildInfo.Name = "lblChildInfo";
            lblChildInfo.Size = new Size(345, 48);
            lblChildInfo.Text = "";

            // ===== grpSummary =====
            grpSummary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpSummary.Location = new Point(20, 150);
            grpSummary.Name = "grpSummary";
            grpSummary.Size = new Size(1060, 110);
            grpSummary.TabStop = false;
            grpSummary.Text = "Vaccination Summary";
            grpSummary.Controls.Add(lblLatest);
            grpSummary.Controls.Add(lblNext);
            grpSummary.Controls.Add(lblStatus);

            lblLatest.AutoSize = true;
            lblLatest.Location = new Point(15, 25);
            lblLatest.Name = "lblLatest";
            lblLatest.Text = "Select a child to see the summary.";

            lblNext.AutoSize = true;
            lblNext.Location = new Point(15, 52);
            lblNext.Name = "lblNext";
            lblNext.Text = "";

            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblStatus.Location = new Point(15, 78);
            lblStatus.Name = "lblStatus";
            lblStatus.Text = "";

            // ===== grpRecord =====
            grpRecord.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpRecord.Location = new Point(20, 270);
            grpRecord.Name = "grpRecord";
            grpRecord.Size = new Size(1060, 150);
            grpRecord.TabStop = false;
            grpRecord.Text = "Record a Dose";
            grpRecord.Controls.Add(lblDose);
            grpRecord.Controls.Add(cboDose);
            grpRecord.Controls.Add(lblDate);
            grpRecord.Controls.Add(dtpDate);
            grpRecord.Controls.Add(lblWorker);
            grpRecord.Controls.Add(txtWorker);
            grpRecord.Controls.Add(lblRemarks);
            grpRecord.Controls.Add(txtRemarks);
            grpRecord.Controls.Add(btnRecord);

            lblDose.AutoSize = true;
            lblDose.Location = new Point(15, 25);
            lblDose.Name = "lblDose";
            lblDose.Text = "Dose to record *";
            cboDose.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDose.Location = new Point(15, 47);
            cboDose.Name = "cboDose";
            cboDose.Size = new Size(480, 25);
            cboDose.TabIndex = 2;

            lblDate.AutoSize = true;
            lblDate.Location = new Point(515, 25);
            lblDate.Name = "lblDate";
            lblDate.Text = "Date Administered *";
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Location = new Point(515, 47);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(180, 25);
            dtpDate.TabIndex = 3;

            lblWorker.AutoSize = true;
            lblWorker.Location = new Point(715, 25);
            lblWorker.Name = "lblWorker";
            lblWorker.Text = "Administered by (logged in)";
            txtWorker.Location = new Point(715, 47);
            txtWorker.Name = "txtWorker";
            txtWorker.ReadOnly = true;
            txtWorker.Size = new Size(330, 25);
            txtWorker.TabIndex = 4;
            txtWorker.TabStop = false;

            lblRemarks.AutoSize = true;
            lblRemarks.Location = new Point(15, 88);
            lblRemarks.Name = "lblRemarks";
            lblRemarks.Text = "Remarks (optional)";
            txtRemarks.Location = new Point(15, 110);
            txtRemarks.MaxLength = 250;
            txtRemarks.Name = "txtRemarks";
            txtRemarks.Size = new Size(880, 25);
            txtRemarks.TabIndex = 5;

            btnRecord.BackColor = Color.FromArgb(0, 121, 107);
            btnRecord.Cursor = Cursors.Hand;
            btnRecord.FlatStyle = FlatStyle.Flat;
            btnRecord.FlatAppearance.BorderSize = 0;
            btnRecord.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRecord.ForeColor = Color.White;
            btnRecord.Location = new Point(910, 104);
            btnRecord.Name = "btnRecord";
            btnRecord.Size = new Size(135, 36);
            btnRecord.TabIndex = 6;
            btnRecord.Text = "Record Dose";
            btnRecord.UseVisualStyleBackColor = false;

            // ===== grids =====
            lblNextTitle.AutoSize = true;
            lblNextTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblNextTitle.ForeColor = Color.FromArgb(0, 105, 92);
            lblNextTitle.Location = new Point(20, 430);
            lblNextTitle.Name = "lblNextTitle";
            lblNextTitle.Text = "Next Required Doses";

            StyleGrid(dgvNextDoses, "dgvNextDoses", new Point(20, 458), new Size(520, 282), 7);
            dgvNextDoses.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;

            lblHistoryTitle.AutoSize = true;
            lblHistoryTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblHistoryTitle.ForeColor = Color.FromArgb(0, 105, 92);
            lblHistoryTitle.Location = new Point(560, 430);
            lblHistoryTitle.Name = "lblHistoryTitle";
            lblHistoryTitle.Text = "Vaccination History";

            StyleGrid(dgvHistory, "dgvHistory", new Point(560, 458), new Size(520, 282), 8);
            dgvHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // VaccinationForm
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1100, 760);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1116, 799);
            Name = "VaccinationForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Vaccination - IMMUNOTRACK";
            Controls.Add(lblTitle);
            Controls.Add(grpChild);
            Controls.Add(grpSummary);
            Controls.Add(grpRecord);
            Controls.Add(lblNextTitle);
            Controls.Add(dgvNextDoses);
            Controls.Add(lblHistoryTitle);
            Controls.Add(dgvHistory);

            ((System.ComponentModel.ISupportInitialize)dgvNextDoses).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvHistory).EndInit();
            grpChild.ResumeLayout(false);
            grpChild.PerformLayout();
            grpSummary.ResumeLayout(false);
            grpSummary.PerformLayout();
            grpRecord.ResumeLayout(false);
            grpRecord.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        // Gives both grids the same look.
        private static void StyleGrid(DataGridView grid, string name, Point location, Size size, int tabIndex)
        {
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.FromArgb(0, 105, 92);
            headerStyle.ForeColor = Color.White;
            headerStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            headerStyle.SelectionBackColor = Color.FromArgb(0, 105, 92);
            headerStyle.SelectionForeColor = Color.White;

            DataGridViewCellStyle alternateStyle = new DataGridViewCellStyle();
            alternateStyle.BackColor = Color.FromArgb(237, 247, 245);

            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AlternatingRowsDefaultCellStyle = alternateStyle;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle = headerStyle;
            grid.ColumnHeadersHeight = 34;
            grid.EnableHeadersVisualStyles = false;
            grid.Location = location;
            grid.MultiSelect = false;
            grid.Name = name;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Size = size;
            grid.TabIndex = tabIndex;
        }

        #endregion

        private Label lblTitle;
        private GroupBox grpChild;
        private Label lblFind;
        private TextBox txtFindChild;
        private Label lblChild;
        private ComboBox cboChild;
        private Label lblChildInfo;
        private GroupBox grpSummary;
        private Label lblLatest;
        private Label lblNext;
        private Label lblStatus;
        private GroupBox grpRecord;
        private Label lblDose;
        private ComboBox cboDose;
        private Label lblDate;
        private DateTimePicker dtpDate;
        private Label lblWorker;
        private TextBox txtWorker;
        private Label lblRemarks;
        private TextBox txtRemarks;
        private Button btnRecord;
        private Label lblNextTitle;
        private DataGridView dgvNextDoses;
        private Label lblHistoryTitle;
        private DataGridView dgvHistory;
    }
}