// Folder: Forms/HealthWorkersForm.Designer.cs
// (This is the layout file: where every textbox, button and grid sits.)
#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace ImmunoTrack.Forms
{
    partial class HealthWorkersForm
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
            lblContact = new Label();
            txtContactNumber = new TextBox();
            lblPosition = new Label();
            cboPosition = new ComboBox();
            lblLicenseNo = new Label();
            txtLicenseNo = new TextBox();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            lblConfirmPassword = new Label();
            txtConfirmPassword = new TextBox();
            lblPasswordNote = new Label();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnClear = new Button();
            btnResetPassword = new Button();
            lblSearch = new Label();
            txtSearch = new TextBox();
            btnSearch = new Button();
            btnShowAll = new Button();
            lblCount = new Label();
            dgvWorkers = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvWorkers).BeginInit();
            grpDetails.SuspendLayout();
            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 105, 92);
            lblTitle.Location = new Point(20, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Health Worker Management";

            // grpDetails
            grpDetails.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpDetails.Location = new Point(20, 55);
            grpDetails.Name = "grpDetails";
            grpDetails.Size = new Size(960, 215);
            grpDetails.TabStop = false;
            grpDetails.Text = "Health Worker Details";
            grpDetails.Controls.Add(lblFirstName);
            grpDetails.Controls.Add(txtFirstName);
            grpDetails.Controls.Add(lblMiddleName);
            grpDetails.Controls.Add(txtMiddleName);
            grpDetails.Controls.Add(lblLastName);
            grpDetails.Controls.Add(txtLastName);
            grpDetails.Controls.Add(lblContact);
            grpDetails.Controls.Add(txtContactNumber);
            grpDetails.Controls.Add(lblPosition);
            grpDetails.Controls.Add(cboPosition);
            grpDetails.Controls.Add(lblLicenseNo);
            grpDetails.Controls.Add(txtLicenseNo);
            grpDetails.Controls.Add(lblUsername);
            grpDetails.Controls.Add(txtUsername);
            grpDetails.Controls.Add(lblPassword);
            grpDetails.Controls.Add(txtPassword);
            grpDetails.Controls.Add(lblConfirmPassword);
            grpDetails.Controls.Add(txtConfirmPassword);

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

            // Row 2: contact, position, license
            lblContact.AutoSize = true;
            lblContact.Location = new Point(15, 88);
            lblContact.Name = "lblContact";
            lblContact.Text = "Contact Number *";
            txtContactNumber.Location = new Point(15, 110);
            txtContactNumber.MaxLength = 20;
            txtContactNumber.Name = "txtContactNumber";
            txtContactNumber.Size = new Size(290, 25);
            txtContactNumber.TabIndex = 3;

            lblPosition.AutoSize = true;
            lblPosition.Location = new Point(335, 88);
            lblPosition.Name = "lblPosition";
            lblPosition.Text = "Position *";
            cboPosition.DropDownStyle = ComboBoxStyle.DropDown;
            cboPosition.Location = new Point(335, 110);
            cboPosition.MaxLength = 50;
            cboPosition.Name = "cboPosition";
            cboPosition.Size = new Size(290, 25);
            cboPosition.TabIndex = 4;

            lblLicenseNo.AutoSize = true;
            lblLicenseNo.Location = new Point(655, 88);
            lblLicenseNo.Name = "lblLicenseNo";
            lblLicenseNo.Text = "License No.";
            txtLicenseNo.Location = new Point(655, 110);
            txtLicenseNo.MaxLength = 30;
            txtLicenseNo.Name = "txtLicenseNo";
            txtLicenseNo.Size = new Size(290, 25);
            txtLicenseNo.TabIndex = 5;

            // Row 3: account
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(15, 148);
            lblUsername.Name = "lblUsername";
            lblUsername.Text = "Username *";
            txtUsername.Location = new Point(15, 170);
            txtUsername.MaxLength = 30;
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(290, 25);
            txtUsername.TabIndex = 6;

            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(335, 148);
            lblPassword.Name = "lblPassword";
            lblPassword.Text = "Password";
            txtPassword.Location = new Point(335, 170);
            txtPassword.MaxLength = 50;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(290, 25);
            txtPassword.TabIndex = 7;
            txtPassword.UseSystemPasswordChar = true;

            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Location = new Point(655, 148);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Text = "Confirm Password";
            txtConfirmPassword.Location = new Point(655, 170);
            txtConfirmPassword.MaxLength = 50;
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(290, 25);
            txtConfirmPassword.TabIndex = 8;
            txtConfirmPassword.UseSystemPasswordChar = true;

            // lblPasswordNote
            lblPasswordNote.AutoSize = true;
            lblPasswordNote.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblPasswordNote.ForeColor = Color.FromArgb(96, 125, 139);
            lblPasswordNote.Location = new Point(20, 276);
            lblPasswordNote.Name = "lblPasswordNote";
            lblPasswordNote.Text = "Password fields are used only when adding a worker or clicking Reset Password (minimum 6 characters).";

            // Buttons
            StyleButton(btnAdd, "btnAdd", "Add", Color.FromArgb(0, 121, 107), new Point(20, 305), new Size(120, 36), 9);
            StyleButton(btnUpdate, "btnUpdate", "Update", Color.FromArgb(0, 121, 107), new Point(150, 305), new Size(120, 36), 10);
            StyleButton(btnDelete, "btnDelete", "Delete", Color.FromArgb(198, 40, 40), new Point(280, 305), new Size(120, 36), 11);
            StyleButton(btnClear, "btnClear", "Clear / New", Color.FromArgb(96, 125, 139), new Point(410, 305), new Size(120, 36), 12);
            StyleButton(btnResetPassword, "btnResetPassword", "Reset Password", Color.FromArgb(239, 108, 0), new Point(540, 305), new Size(150, 36), 13);

            // Search row
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(20, 363);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Search name:";

            txtSearch.Location = new Point(125, 359);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(300, 25);
            txtSearch.TabIndex = 14;

            StyleButton(btnSearch, "btnSearch", "Search", Color.FromArgb(0, 121, 107), new Point(435, 356), new Size(100, 32), 15);
            StyleButton(btnShowAll, "btnShowAll", "Show All", Color.FromArgb(96, 125, 139), new Point(545, 356), new Size(100, 32), 16);

            lblCount.AutoSize = true;
            lblCount.ForeColor = Color.FromArgb(96, 125, 139);
            lblCount.Location = new Point(665, 363);
            lblCount.Name = "lblCount";
            lblCount.Text = "";

            // dgvWorkers
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.FromArgb(0, 105, 92);
            headerStyle.ForeColor = Color.White;
            headerStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            headerStyle.SelectionBackColor = Color.FromArgb(0, 105, 92);
            headerStyle.SelectionForeColor = Color.White;

            DataGridViewCellStyle alternateStyle = new DataGridViewCellStyle();
            alternateStyle.BackColor = Color.FromArgb(237, 247, 245);

            dgvWorkers.AllowUserToAddRows = false;
            dgvWorkers.AllowUserToDeleteRows = false;
            dgvWorkers.AlternatingRowsDefaultCellStyle = alternateStyle;
            dgvWorkers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvWorkers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvWorkers.BackgroundColor = Color.White;
            dgvWorkers.BorderStyle = BorderStyle.None;
            dgvWorkers.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvWorkers.ColumnHeadersHeight = 34;
            dgvWorkers.EnableHeadersVisualStyles = false;
            dgvWorkers.Location = new Point(20, 405);
            dgvWorkers.MultiSelect = false;
            dgvWorkers.Name = "dgvWorkers";
            dgvWorkers.ReadOnly = true;
            dgvWorkers.RowHeadersVisible = false;
            dgvWorkers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvWorkers.Size = new Size(960, 275);
            dgvWorkers.TabIndex = 17;

            // HealthWorkersForm
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1000, 700);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1016, 739);
            Name = "HealthWorkersForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Health Worker Management - IMMUNOTRACK";
            Controls.Add(lblTitle);
            Controls.Add(grpDetails);
            Controls.Add(lblPasswordNote);
            Controls.Add(btnAdd);
            Controls.Add(btnUpdate);
            Controls.Add(btnDelete);
            Controls.Add(btnClear);
            Controls.Add(btnResetPassword);
            Controls.Add(lblSearch);
            Controls.Add(txtSearch);
            Controls.Add(btnSearch);
            Controls.Add(btnShowAll);
            Controls.Add(lblCount);
            Controls.Add(dgvWorkers);

            ((System.ComponentModel.ISupportInitialize)dgvWorkers).EndInit();
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
        private Label lblContact;
        private TextBox txtContactNumber;
        private Label lblPosition;
        private ComboBox cboPosition;
        private Label lblLicenseNo;
        private TextBox txtLicenseNo;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Label lblConfirmPassword;
        private TextBox txtConfirmPassword;
        private Label lblPasswordNote;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;
        private Button btnResetPassword;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnShowAll;
        private Label lblCount;
        private DataGridView dgvWorkers;
    }
}