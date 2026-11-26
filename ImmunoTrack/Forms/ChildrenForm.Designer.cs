// Folder: Forms/ChildrenForm.Designer.cs
// (This is the layout file: where every textbox, button and grid sits.)
#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace ImmunoTrack.Forms
{
    partial class ChildrenForm
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
            grpDetails = new GroupBox();
            lblFirstName = new Label();
            txtFirstName = new TextBox();
            lblMiddleName = new Label();
            txtMiddleName = new TextBox();
            lblLastName = new Label();
            txtLastName = new TextBox();
            lblDateOfBirth = new Label();
            dtpDateOfBirth = new DateTimePicker();
            lblSex = new Label();
            cboSex = new ComboBox();
            lblBloodType = new Label();
            cboBloodType = new ComboBox();
            lblGuardian = new Label();
            cboGuardian = new ComboBox();
            lblAddress = new Label();
            txtAddress = new TextBox();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnClear = new Button();
            lblSearchBy = new Label();
            cboSearchBy = new ComboBox();
            txtSearch = new TextBox();
            btnSearch = new Button();
            btnShowAll = new Button();
            lblCount = new Label();
            dgvChildren = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvChildren).BeginInit();
            grpDetails.SuspendLayout();
            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 105, 92);
            lblTitle.Location = new Point(20, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Children Management";

            // grpDetails
            grpDetails.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpDetails.Location = new Point(20, 55);
            grpDetails.Name = "grpDetails";
            grpDetails.Size = new Size(960, 215);
            grpDetails.TabStop = false;
            grpDetails.Text = "Child Details";
            grpDetails.Controls.Add(lblFirstName);
            grpDetails.Controls.Add(txtFirstName);
            grpDetails.Controls.Add(lblMiddleName);
            grpDetails.Controls.Add(txtMiddleName);
            grpDetails.Controls.Add(lblLastName);
            grpDetails.Controls.Add(txtLastName);
            grpDetails.Controls.Add(lblDateOfBirth);
            grpDetails.Controls.Add(dtpDateOfBirth);
            grpDetails.Controls.Add(lblSex);
            grpDetails.Controls.Add(cboSex);
            grpDetails.Controls.Add(lblBloodType);
            grpDetails.Controls.Add(cboBloodType);
            grpDetails.Controls.Add(lblGuardian);
            grpDetails.Controls.Add(cboGuardian);
            grpDetails.Controls.Add(lblAddress);
            grpDetails.Controls.Add(txtAddress);

            // Row 1: names
            lblFirstName.AutoSize = true;
            lblFirstName.Location = new Point(15, 28);
            lblFirstName.Name = "lblFirstName";
            lblFirstName.Text = "First Name *";
            txtFirstName.Location = new Point(15, 50);
            txtFirstName.MaxLength = 50;
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(290, 25);
            txtFirstName.TabIndex = 0;

            lblMiddleName.AutoSize = true;
            lblMiddleName.Location = new Point(335, 28);
            lblMiddleName.Name = "lblMiddleName";
            lblMiddleName.Text = "Middle Name";
            txtMiddleName.Location = new Point(335, 50);
            txtMiddleName.MaxLength = 50;
            txtMiddleName.Name = "txtMiddleName";
            txtMiddleName.Size = new Size(290, 25);
            txtMiddleName.TabIndex = 1;

            lblLastName.AutoSize = true;
            lblLastName.Location = new Point(655, 28);
            lblLastName.Name = "lblLastName";
            lblLastName.Text = "Last Name *";
            txtLastName.Location = new Point(655, 50);
            txtLastName.MaxLength = 50;
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(290, 25);
            txtLastName.TabIndex = 2;

            // Row 2: date of birth, sex, blood type
            lblDateOfBirth.AutoSize = true;
            lblDateOfBirth.Location = new Point(15, 88);
            lblDateOfBirth.Name = "lblDateOfBirth";
            lblDateOfBirth.Text = "Date of Birth *";
            dtpDateOfBirth.Format = DateTimePickerFormat.Short;
            dtpDateOfBirth.Location = new Point(15, 110);
            dtpDateOfBirth.Name = "dtpDateOfBirth";
            dtpDateOfBirth.Size = new Size(290, 25);
            dtpDateOfBirth.TabIndex = 3;

            lblSex.AutoSize = true;
            lblSex.Location = new Point(335, 88);
            lblSex.Name = "lblSex";
            lblSex.Text = "Sex *";
            cboSex.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSex.Location = new Point(335, 110);
            cboSex.Name = "cboSex";
            cboSex.Size = new Size(290, 25);
            cboSex.TabIndex = 4;

            lblBloodType.AutoSize = true;
            lblBloodType.Location = new Point(655, 88);
            lblBloodType.Name = "lblBloodType";
            lblBloodType.Text = "Blood Type";
            cboBloodType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBloodType.Location = new Point(655, 110);
            cboBloodType.Name = "cboBloodType";
            cboBloodType.Size = new Size(290, 25);
            cboBloodType.TabIndex = 5;

            // Row 3: guardian, address
            lblGuardian.AutoSize = true;
            lblGuardian.Location = new Point(15, 148);
            lblGuardian.Name = "lblGuardian";
            lblGuardian.Text = "Guardian *";
            cboGuardian.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGuardian.Location = new Point(15, 170);
            cboGuardian.Name = "cboGuardian";
            cboGuardian.Size = new Size(290, 25);
            cboGuardian.TabIndex = 6;

            lblAddress.AutoSize = true;
            lblAddress.Location = new Point(335, 148);
            lblAddress.Name = "lblAddress";
            lblAddress.Text = "Address *";
            txtAddress.Location = new Point(335, 170);
            txtAddress.MaxLength = 200;
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(610, 25);
            txtAddress.TabIndex = 7;

            // Buttons
            StyleButton(btnAdd, "btnAdd", "Add", Color.FromArgb(0, 121, 107), new Point(20, 285), new Size(120, 36), 8);
            StyleButton(btnUpdate, "btnUpdate", "Update", Color.FromArgb(0, 121, 107), new Point(150, 285), new Size(120, 36), 9);
            StyleButton(btnDelete, "btnDelete", "Delete", Color.FromArgb(198, 40, 40), new Point(280, 285), new Size(120, 36), 10);
            StyleButton(btnClear, "btnClear", "Clear / New", Color.FromArgb(96, 125, 139), new Point(410, 285), new Size(120, 36), 11);

            // Search row
            lblSearchBy.AutoSize = true;
            lblSearchBy.Location = new Point(20, 343);
            lblSearchBy.Name = "lblSearchBy";
            lblSearchBy.Text = "Search by:";

            cboSearchBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSearchBy.Location = new Point(100, 339);
            cboSearchBy.Name = "cboSearchBy";
            cboSearchBy.Size = new Size(150, 25);
            cboSearchBy.TabIndex = 12;

            txtSearch.Location = new Point(260, 339);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(300, 25);
            txtSearch.TabIndex = 13;

            StyleButton(btnSearch, "btnSearch", "Search", Color.FromArgb(0, 121, 107), new Point(570, 336), new Size(100, 32), 14);
            StyleButton(btnShowAll, "btnShowAll", "Show All", Color.FromArgb(96, 125, 139), new Point(680, 336), new Size(100, 32), 15);

            lblCount.AutoSize = true;
            lblCount.ForeColor = Color.FromArgb(96, 125, 139);
            lblCount.Location = new Point(800, 343);
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
            dgvChildren.Location = new Point(20, 385);
            dgvChildren.MultiSelect = false;
            dgvChildren.Name = "dgvChildren";
            dgvChildren.ReadOnly = true;
            dgvChildren.RowHeadersVisible = false;
            dgvChildren.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChildren.Size = new Size(960, 295);
            dgvChildren.TabIndex = 16;

            // ChildrenForm
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1000, 700);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1016, 739);
            Name = "ChildrenForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Children Management - IMMUNOTRACK";
            Controls.Add(lblTitle);
            Controls.Add(grpDetails);
            Controls.Add(btnAdd);
            Controls.Add(btnUpdate);
            Controls.Add(btnDelete);
            Controls.Add(btnClear);
            Controls.Add(lblSearchBy);
            Controls.Add(cboSearchBy);
            Controls.Add(txtSearch);
            Controls.Add(btnSearch);
            Controls.Add(btnShowAll);
            Controls.Add(lblCount);
            Controls.Add(dgvChildren);

            ((System.ComponentModel.ISupportInitialize)dgvChildren).EndInit();
            grpDetails.ResumeLayout(false);
            grpDetails.PerformLayout();
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

        #endregion

        private Label lblTitle;
        private GroupBox grpDetails;
        private Label lblFirstName;
        private TextBox txtFirstName;
        private Label lblMiddleName;
        private TextBox txtMiddleName;
        private Label lblLastName;
        private TextBox txtLastName;
        private Label lblDateOfBirth;
        private DateTimePicker dtpDateOfBirth;
        private Label lblSex;
        private ComboBox cboSex;
        private Label lblBloodType;
        private ComboBox cboBloodType;
        private Label lblGuardian;
        private ComboBox cboGuardian;
        private Label lblAddress;
        private TextBox txtAddress;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;
        private Label lblSearchBy;
        private ComboBox cboSearchBy;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnShowAll;
        private Label lblCount;
        private DataGridView dgvChildren;
    }
}