// Folder: Forms/GuardiansForm.Designer.cs
// (This is the layout file: where every textbox, button and grid sits.)
#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace ImmunoTrack.Forms
{
    partial class GuardiansForm
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
            lblRelationship = new Label();
            cboRelationship = new ComboBox();
            lblAddress = new Label();
            txtAddress = new TextBox();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnClear = new Button();
            lblSearch = new Label();
            txtSearch = new TextBox();
            btnSearch = new Button();
            btnShowAll = new Button();
            lblCount = new Label();
            dgvGuardians = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvGuardians).BeginInit();
            grpDetails.SuspendLayout();
            SuspendLayout();

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 105, 92);
            lblTitle.Location = new Point(20, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Text = "Guardian Management";

            // grpDetails
            grpDetails.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpDetails.Location = new Point(20, 55);
            grpDetails.Name = "grpDetails";
            grpDetails.Size = new Size(860, 205);
            grpDetails.TabStop = false;
            grpDetails.Text = "Guardian Details";
            grpDetails.Controls.Add(lblFirstName);
            grpDetails.Controls.Add(txtFirstName);
            grpDetails.Controls.Add(lblMiddleName);
            grpDetails.Controls.Add(txtMiddleName);
            grpDetails.Controls.Add(lblLastName);
            grpDetails.Controls.Add(txtLastName);
            grpDetails.Controls.Add(lblContact);
            grpDetails.Controls.Add(txtContactNumber);
            grpDetails.Controls.Add(lblRelationship);
            grpDetails.Controls.Add(cboRelationship);
            grpDetails.Controls.Add(lblAddress);
            grpDetails.Controls.Add(txtAddress);

            // First name
            lblFirstName.AutoSize = true;
            lblFirstName.Location = new Point(15, 28);
            lblFirstName.Name = "lblFirstName";
            lblFirstName.Text = "First Name *";
            txtFirstName.Location = new Point(15, 50);
            txtFirstName.MaxLength = 50;
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(260, 25);
            txtFirstName.TabIndex = 0;

            // Middle name
            lblMiddleName.AutoSize = true;
            lblMiddleName.Location = new Point(295, 28);
            lblMiddleName.Name = "lblMiddleName";
            lblMiddleName.Text = "Middle Name";
            txtMiddleName.Location = new Point(295, 50);
            txtMiddleName.MaxLength = 50;
            txtMiddleName.Name = "txtMiddleName";
            txtMiddleName.Size = new Size(260, 25);
            txtMiddleName.TabIndex = 1;

            // Last name
            lblLastName.AutoSize = true;
            lblLastName.Location = new Point(575, 28);
            lblLastName.Name = "lblLastName";
            lblLastName.Text = "Last Name *";
            txtLastName.Location = new Point(575, 50);
            txtLastName.MaxLength = 50;
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(260, 25);
            txtLastName.TabIndex = 2;

            // Contact number
            lblContact.AutoSize = true;
            lblContact.Location = new Point(15, 88);
            lblContact.Name = "lblContact";
            lblContact.Text = "Contact Number *";
            txtContactNumber.Location = new Point(15, 110);
            txtContactNumber.MaxLength = 20;
            txtContactNumber.Name = "txtContactNumber";
            txtContactNumber.Size = new Size(260, 25);
            txtContactNumber.TabIndex = 3;

            // Relationship
            lblRelationship.AutoSize = true;
            lblRelationship.Location = new Point(295, 88);
            lblRelationship.Name = "lblRelationship";
            lblRelationship.Text = "Relationship to Child *";
            cboRelationship.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRelationship.Location = new Point(295, 110);
            cboRelationship.Name = "cboRelationship";
            cboRelationship.Size = new Size(260, 25);
            cboRelationship.TabIndex = 4;

            // Address
            lblAddress.AutoSize = true;
            lblAddress.Location = new Point(15, 148);
            lblAddress.Name = "lblAddress";
            lblAddress.Text = "Address *";
            txtAddress.Location = new Point(15, 170);
            txtAddress.MaxLength = 200;
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(820, 25);
            txtAddress.TabIndex = 5;

            // btnAdd
            StyleButton(btnAdd, "btnAdd", "Add", Color.FromArgb(0, 121, 107), new Point(20, 275), new Size(120, 36), 6);

            // btnUpdate
            StyleButton(btnUpdate, "btnUpdate", "Update", Color.FromArgb(0, 121, 107), new Point(150, 275), new Size(120, 36), 7);

            // btnDelete
            StyleButton(btnDelete, "btnDelete", "Delete", Color.FromArgb(198, 40, 40), new Point(280, 275), new Size(120, 36), 8);

            // btnClear
            StyleButton(btnClear, "btnClear", "Clear / New", Color.FromArgb(96, 125, 139), new Point(410, 275), new Size(120, 36), 9);

            // lblSearch
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(20, 333);
            lblSearch.Name = "lblSearch";
            lblSearch.Text = "Search name:";

            // txtSearch
            txtSearch.Location = new Point(125, 329);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(300, 25);
            txtSearch.TabIndex = 10;

            // btnSearch
            StyleButton(btnSearch, "btnSearch", "Search", Color.FromArgb(0, 121, 107), new Point(435, 326), new Size(100, 32), 11);

            // btnShowAll
            StyleButton(btnShowAll, "btnShowAll", "Show All", Color.FromArgb(96, 125, 139), new Point(545, 326), new Size(100, 32), 12);

            // lblCount
            lblCount.AutoSize = true;
            lblCount.ForeColor = Color.FromArgb(96, 125, 139);
            lblCount.Location = new Point(665, 333);
            lblCount.Name = "lblCount";
            lblCount.Text = "";

            // dgvGuardians
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.FromArgb(0, 105, 92);
            headerStyle.ForeColor = Color.White;
            headerStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            headerStyle.SelectionBackColor = Color.FromArgb(0, 105, 92);
            headerStyle.SelectionForeColor = Color.White;

            DataGridViewCellStyle alternateStyle = new DataGridViewCellStyle();
            alternateStyle.BackColor = Color.FromArgb(237, 247, 245);

            dgvGuardians.AllowUserToAddRows = false;
            dgvGuardians.AllowUserToDeleteRows = false;
            dgvGuardians.AlternatingRowsDefaultCellStyle = alternateStyle;
            dgvGuardians.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvGuardians.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGuardians.BackgroundColor = Color.White;
            dgvGuardians.BorderStyle = BorderStyle.None;
            dgvGuardians.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvGuardians.ColumnHeadersHeight = 34;
            dgvGuardians.EnableHeadersVisualStyles = false;
            dgvGuardians.Location = new Point(20, 375);
            dgvGuardians.MultiSelect = false;
            dgvGuardians.Name = "dgvGuardians";
            dgvGuardians.ReadOnly = true;
            dgvGuardians.RowHeadersVisible = false;
            dgvGuardians.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGuardians.Size = new Size(860, 245);
            dgvGuardians.TabIndex = 13;

            // GuardiansForm
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(900, 640);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(916, 679);
            Name = "GuardiansForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Guardian Management - IMMUNOTRACK";
            Controls.Add(lblTitle);
            Controls.Add(grpDetails);
            Controls.Add(btnAdd);
            Controls.Add(btnUpdate);
            Controls.Add(btnDelete);
            Controls.Add(btnClear);
            Controls.Add(lblSearch);
            Controls.Add(txtSearch);
            Controls.Add(btnSearch);
            Controls.Add(btnShowAll);
            Controls.Add(lblCount);
            Controls.Add(dgvGuardians);

            ((System.ComponentModel.ISupportInitialize)dgvGuardians).EndInit();
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
        private Label lblRelationship;
        private ComboBox cboRelationship;
        private Label lblAddress;
        private TextBox txtAddress;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnShowAll;
        private Label lblCount;
        private DataGridView dgvGuardians;
    }
}