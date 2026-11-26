// Folder: Forms/VaccinesForm.Designer.cs
// (This is the layout file.)
#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace ImmunoTrack.Forms
{
    partial class VaccinesForm
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
            grpVaccine = new GroupBox();
            lblVaccineName = new Label();
            txtVaccineName = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            chkVaccineActive = new CheckBox();
            btnAddVaccine = new Button();
            btnUpdateVaccine = new Button();
            btnDeleteVaccine = new Button();
            btnClearVaccine = new Button();
            dgvVaccines = new DataGridView();
            grpSchedule = new GroupBox();
            lblDose = new Label();
            nudDoseNumber = new NumericUpDown();
            lblAge = new Label();
            nudAgeDays = new NumericUpDown();
            lblInterval = new Label();
            txtInterval = new TextBox();
            chkScheduleActive = new CheckBox();
            btnAddDose = new Button();
            btnUpdateDose = new Button();
            btnDeleteDose = new Button();
            btnClearDose = new Button();
            lblScheduleHint = new Label();
            dgvSchedule = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvVaccines).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvSchedule).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDoseNumber).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudAgeDays).BeginInit();
            grpVaccine.SuspendLayout();
            grpSchedule.SuspendLayout();
            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 105, 92);
            lblTitle.Location = new Point(20, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Vaccines and Dose Schedule";

            // ===== Vaccine group =====
            grpVaccine.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpVaccine.Location = new Point(20, 55);
            grpVaccine.Name = "grpVaccine";
            grpVaccine.Size = new Size(1060, 135);
            grpVaccine.TabStop = false;
            grpVaccine.Text = "Vaccine";
            grpVaccine.Controls.Add(lblVaccineName);
            grpVaccine.Controls.Add(txtVaccineName);
            grpVaccine.Controls.Add(lblDescription);
            grpVaccine.Controls.Add(txtDescription);
            grpVaccine.Controls.Add(chkVaccineActive);
            grpVaccine.Controls.Add(btnAddVaccine);
            grpVaccine.Controls.Add(btnUpdateVaccine);
            grpVaccine.Controls.Add(btnDeleteVaccine);
            grpVaccine.Controls.Add(btnClearVaccine);

            lblVaccineName.AutoSize = true;
            lblVaccineName.Location = new Point(15, 25);
            lblVaccineName.Name = "lblVaccineName";
            lblVaccineName.Text = "Vaccine Name *";
            txtVaccineName.Location = new Point(15, 47);
            txtVaccineName.MaxLength = 100;
            txtVaccineName.Name = "txtVaccineName";
            txtVaccineName.Size = new Size(300, 25);
            txtVaccineName.TabIndex = 0;

            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(335, 25);
            lblDescription.Name = "lblDescription";
            lblDescription.Text = "Description";
            txtDescription.Location = new Point(335, 47);
            txtDescription.MaxLength = 250;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(450, 25);
            txtDescription.TabIndex = 1;

            chkVaccineActive.AutoSize = true;
            chkVaccineActive.Checked = true;
            chkVaccineActive.Location = new Point(805, 49);
            chkVaccineActive.Name = "chkVaccineActive";
            chkVaccineActive.TabIndex = 2;
            chkVaccineActive.Text = "Active";

            StyleButton(btnAddVaccine, "btnAddVaccine", "Add", Color.FromArgb(0, 121, 107), new Point(15, 88), new Size(110, 34), 3);
            StyleButton(btnUpdateVaccine, "btnUpdateVaccine", "Update", Color.FromArgb(0, 121, 107), new Point(135, 88), new Size(110, 34), 4);
            StyleButton(btnDeleteVaccine, "btnDeleteVaccine", "Delete", Color.FromArgb(198, 40, 40), new Point(255, 88), new Size(110, 34), 5);
            StyleButton(btnClearVaccine, "btnClearVaccine", "Clear / New", Color.FromArgb(96, 125, 139), new Point(375, 88), new Size(110, 34), 6);

            // dgvVaccines
            StyleGrid(dgvVaccines, "dgvVaccines", new Point(20, 200), new Size(1060, 140), 7);
            dgvVaccines.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // ===== Schedule group =====
            grpSchedule.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpSchedule.Location = new Point(20, 355);
            grpSchedule.Name = "grpSchedule";
            grpSchedule.Size = new Size(1060, 135);
            grpSchedule.TabStop = false;
            grpSchedule.Text = "Dose Schedule";
            grpSchedule.Controls.Add(lblDose);
            grpSchedule.Controls.Add(nudDoseNumber);
            grpSchedule.Controls.Add(lblAge);
            grpSchedule.Controls.Add(nudAgeDays);
            grpSchedule.Controls.Add(lblInterval);
            grpSchedule.Controls.Add(txtInterval);
            grpSchedule.Controls.Add(chkScheduleActive);
            grpSchedule.Controls.Add(btnAddDose);
            grpSchedule.Controls.Add(btnUpdateDose);
            grpSchedule.Controls.Add(btnDeleteDose);
            grpSchedule.Controls.Add(btnClearDose);

            lblDose.AutoSize = true;
            lblDose.Location = new Point(15, 25);
            lblDose.Name = "lblDose";
            lblDose.Text = "Dose Number *";
            nudDoseNumber.Location = new Point(15, 47);
            nudDoseNumber.Maximum = 20;
            nudDoseNumber.Minimum = 1;
            nudDoseNumber.Name = "nudDoseNumber";
            nudDoseNumber.Size = new Size(120, 25);
            nudDoseNumber.TabIndex = 8;
            nudDoseNumber.Value = 1;

            lblAge.AutoSize = true;
            lblAge.Location = new Point(155, 25);
            lblAge.Name = "lblAge";
            lblAge.Text = "Recommended Age (days) *";
            nudAgeDays.Location = new Point(155, 47);
            nudAgeDays.Maximum = 36500;
            nudAgeDays.Minimum = 0;
            nudAgeDays.Name = "nudAgeDays";
            nudAgeDays.Size = new Size(200, 25);
            nudAgeDays.TabIndex = 9;

            lblInterval.AutoSize = true;
            lblInterval.Location = new Point(375, 25);
            lblInterval.Name = "lblInterval";
            lblInterval.Text = "Min. Interval after previous dose (days)";
            txtInterval.Location = new Point(375, 47);
            txtInterval.MaxLength = 5;
            txtInterval.Name = "txtInterval";
            txtInterval.Size = new Size(280, 25);
            txtInterval.TabIndex = 10;

            chkScheduleActive.AutoSize = true;
            chkScheduleActive.Checked = true;
            chkScheduleActive.Location = new Point(680, 49);
            chkScheduleActive.Name = "chkScheduleActive";
            chkScheduleActive.TabIndex = 11;
            chkScheduleActive.Text = "Active";

            StyleButton(btnAddDose, "btnAddDose", "Add Dose", Color.FromArgb(0, 121, 107), new Point(15, 88), new Size(110, 34), 12);
            StyleButton(btnUpdateDose, "btnUpdateDose", "Update Dose", Color.FromArgb(0, 121, 107), new Point(135, 88), new Size(120, 34), 13);
            StyleButton(btnDeleteDose, "btnDeleteDose", "Delete Dose", Color.FromArgb(198, 40, 40), new Point(265, 88), new Size(120, 34), 14);
            StyleButton(btnClearDose, "btnClearDose", "Clear / New", Color.FromArgb(96, 125, 139), new Point(395, 88), new Size(110, 34), 15);

            // lblScheduleHint
            lblScheduleHint.AutoSize = true;
            lblScheduleHint.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblScheduleHint.ForeColor = Color.FromArgb(96, 125, 139);
            lblScheduleHint.Location = new Point(20, 495);
            lblScheduleHint.Name = "lblScheduleHint";
            lblScheduleHint.Text = "Ages and intervals are in DAYS. Leave the interval blank for dose 1. The dose number cannot be changed after a dose is created.";

            // dgvSchedule
            StyleGrid(dgvSchedule, "dgvSchedule", new Point(20, 520), new Size(1060, 200), 16);
            dgvSchedule.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            // VaccinesForm
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1100, 740);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1116, 779);
            Name = "VaccinesForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Vaccines and Dose Schedule - IMMUNOTRACK";
            Controls.Add(lblTitle);
            Controls.Add(grpVaccine);
            Controls.Add(dgvVaccines);
            Controls.Add(grpSchedule);
            Controls.Add(lblScheduleHint);
            Controls.Add(dgvSchedule);

            ((System.ComponentModel.ISupportInitialize)dgvVaccines).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvSchedule).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDoseNumber).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudAgeDays).EndInit();
            grpVaccine.ResumeLayout(false);
            grpVaccine.PerformLayout();
            grpSchedule.ResumeLayout(false);
            grpSchedule.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        // Gives every button the same look (consistent UI).
        private static void StyleButton(Button button, string name, string text, Color color,
                                        Point location, Size size, int tabIndex)
        {
            button.BackColor = color;
            button.Cursor = Cursors.Hand;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button.ForeColor = Color.White;
            button.Location = location;
            button.Name = name;
            button.Size = size;
            button.TabIndex = tabIndex;
            button.Text = text;
            button.UseVisualStyleBackColor = false;
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
        private GroupBox grpVaccine;
        private Label lblVaccineName;
        private TextBox txtVaccineName;
        private Label lblDescription;
        private TextBox txtDescription;
        private CheckBox chkVaccineActive;
        private Button btnAddVaccine;
        private Button btnUpdateVaccine;
        private Button btnDeleteVaccine;
        private Button btnClearVaccine;
        private DataGridView dgvVaccines;
        private GroupBox grpSchedule;
        private Label lblDose;
        private NumericUpDown nudDoseNumber;
        private Label lblAge;
        private NumericUpDown nudAgeDays;
        private Label lblInterval;
        private TextBox txtInterval;
        private CheckBox chkScheduleActive;
        private Button btnAddDose;
        private Button btnUpdateDose;
        private Button btnDeleteDose;
        private Button btnClearDose;
        private Label lblScheduleHint;
        private DataGridView dgvSchedule;
    }
}