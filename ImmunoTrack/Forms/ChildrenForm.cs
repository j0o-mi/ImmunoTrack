// Folder: Forms/ChildrenForm.cs
// (This is the behavior file: what happens when you click things.)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ImmunoTrack.Models;
using ImmunoTrack.Services;

namespace ImmunoTrack.Forms
{
    /// <summary>
    /// Children management screen. PRESENTATION LAYER only: it reads the controls,
    /// calls the services, and shows results. No SQL and no business rules here.
    /// </summary>
    public partial class ChildrenForm : Form
    {
        private readonly ChildService _childService = new ChildService();
        private readonly GuardianService _guardianService = new GuardianService();

        private List<Child> _children = new List<Child>();
        private List<Guardian> _guardians = new List<Guardian>();
        private int _selectedChildId = 0;   // 0 = nothing selected (adding a new child)
        private bool _isLoading = false;    // stops the grid selection event while reloading

        public ChildrenForm()
        {
            InitializeComponent();

            cboSex.Items.AddRange(new object[] { "Male", "Female" });

            // Index 0 means "not recorded" and is saved as NULL.
            cboBloodType.Items.AddRange(new object[]
                { "Unknown", "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" });

            // The order here matches the ChildSearchType enum (ChildID, ChildName, GuardianName).
            cboSearchBy.Items.AddRange(new object[] { "Child ID", "Child Name", "Guardian Name" });
            cboSearchBy.SelectedIndex = 1;

            dtpDateOfBirth.MaxDate = DateTime.Today;   // a birth date cannot be in the future

            Load += ChildrenForm_Load;
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            btnSearch.Click += btnSearch_Click;
            btnShowAll.Click += btnShowAll_Click;
            dgvChildren.SelectionChanged += dgvChildren_SelectionChanged;
            cboGuardian.SelectionChangeCommitted += cboGuardian_SelectionChangeCommitted;
            txtSearch.KeyDown += txtSearch_KeyDown;
        }

        // ---------- form events ----------

        private void ChildrenForm_Load(object? sender, EventArgs e)
        {
            LoadGuardianDropdown();
            LoadChildren();
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            try
            {
                Child child = BuildChildFromInputs();
                _childService.AddChild(child);

                MessageBox.Show("Child registered successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtSearch.Clear();
                LoadChildren();
            }
            catch (ArgumentException ex)          // invalid input, thrown by the Child class
            {
                MessageBox.Show(ex.Message, "Please check your input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)  // business rule, e.g. duplicate child
            {
                MessageBox.Show(ex.Message, "Cannot save",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("The child could not be added.", ex);
            }
        }

        private void btnUpdate_Click(object? sender, EventArgs e)
        {
            if (_selectedChildId == 0)
            {
                MessageBox.Show("Click a child in the list first.", "Nothing selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                Child child = BuildChildFromInputs();
                _childService.UpdateChild(child);

                MessageBox.Show("Child updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadChildren();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Please check your input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Cannot save",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("The child could not be updated.", ex);
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedChildId == 0)
            {
                MessageBox.Show("Click a child in the list first.", "Nothing selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string name = txtFirstName.Text + " " + txtLastName.Text;
            DialogResult answer = MessageBox.Show(
                $"Delete {name}? This cannot be undone.", "Confirm delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (answer != DialogResult.Yes)
                return;

            try
            {
                _childService.DeleteChild(_selectedChildId);

                MessageBox.Show("Child deleted.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadChildren();
            }
            catch (InvalidOperationException ex)   // e.g. the child has vaccination records
            {
                MessageBox.Show(ex.Message, "Cannot delete",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("The child could not be deleted.", ex);
            }
        }

        private void btnClear_Click(object? sender, EventArgs e)
        {
            dgvChildren.ClearSelection();
            ClearInputs();
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            LoadChildren();
        }

        private void btnShowAll_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadChildren();
        }

        private void txtSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoadChildren();
            }
        }

        // Convenience: picking a guardian fills the address if it is still empty.
        private void cboGuardian_SelectionChangeCommitted(object? sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtAddress.Text) || cboGuardian.SelectedValue == null)
                return;

            int guardianId = (int)cboGuardian.SelectedValue;
            Guardian? guardian = _guardians.FirstOrDefault(g => g.GuardianID == guardianId);
            if (guardian != null)
                txtAddress.Text = guardian.Address;
        }

        // When a row is clicked, copy that child into the controls for editing.
        private void dgvChildren_SelectionChanged(object? sender, EventArgs e)
        {
            if (_isLoading || dgvChildren.SelectedRows.Count == 0)
                return;

            int id = (int)dgvChildren.SelectedRows[0].Cells["ID"].Value;
            Child? child = _children.FirstOrDefault(c => c.ChildID == id);
            if (child == null)
                return;

            _selectedChildId = child.ChildID;
            txtFirstName.Text = child.FirstName;
            txtMiddleName.Text = child.MiddleName ?? string.Empty;
            txtLastName.Text = child.LastName;
            dtpDateOfBirth.Value = child.DateOfBirth;
            cboSex.SelectedIndex = cboSex.FindStringExact(child.Sex);
            cboBloodType.SelectedIndex = child.BloodType == null
                ? 0
                : cboBloodType.FindStringExact(child.BloodType);
            cboGuardian.SelectedValue = child.GuardianID;
            txtAddress.Text = child.Address;

            SetEditMode(true);
        }

        // ---------- helper methods ----------

        // Fills the guardian dropdown from the database.
        private void LoadGuardianDropdown()
        {
            try
            {
                _guardians = _guardianService.GetAllGuardians();

                cboGuardian.DisplayMember = "Label";
                cboGuardian.ValueMember = "GuardianID";
                cboGuardian.DataSource = _guardians.Select(g => new
                {
                    g.GuardianID,
                    Label = g.FullName + " (" + g.RelationshipToChild + ")"
                }).ToList();
                cboGuardian.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ShowError("The guardian list could not be loaded.", ex);
            }
        }

        // Loads children (all, or filtered by the search box) into the grid.
        private void LoadChildren()
        {
            try
            {
                _isLoading = true;
                _children = _childService.SearchChildren(GetSelectedSearchType(), txtSearch.Text);

                dgvChildren.DataSource = _children.Select(c => new
                {
                    ID = c.ChildID,
                    Name = c.FullName,
                    BirthDate = c.DateOfBirth.ToString("yyyy-MM-dd"),
                    Age = c.GetAgeText(),
                    Sex = c.Sex,
                    BloodType = c.BloodType ?? "-",
                    Guardian = GetGuardianName(c.GuardianID),
                    Address = c.Address
                }).ToList();

                SetColumn("BirthDate", "Date of Birth", 80);
                SetColumn("BloodType", "Blood Type", 60);
                SetColumn("ID", "ID", 35);
                SetColumn("Age", "Age", 60);
                SetColumn("Sex", "Sex", 55);

                dgvChildren.ClearSelection();
                lblCount.Text = _children.Count == 1
                    ? "1 child shown"
                    : $"{_children.Count} children shown";
            }
            catch (ArgumentException ex)    // e.g. a non-numeric Child ID search
            {
                MessageBox.Show(ex.Message, "Please check your search",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("The children list could not be loaded.", ex);
            }
            finally
            {
                _isLoading = false;
            }

            ClearInputs();
        }

        // Creates a Child object from the controls. Each assignment runs the validation
        // inside the Child class, so bad input throws an ArgumentException.
        private Child BuildChildFromInputs()
        {
            if (cboGuardian.SelectedIndex < 0 || cboGuardian.SelectedValue == null)
                throw new ArgumentException("Please select a guardian.");

            Child child = new Child();
            child.ChildID = _selectedChildId;
            child.FirstName = txtFirstName.Text;
            child.MiddleName = txtMiddleName.Text;
            child.LastName = txtLastName.Text;
            child.DateOfBirth = dtpDateOfBirth.Value;
            child.Sex = cboSex.Text;
            child.BloodType = cboBloodType.SelectedIndex <= 0 ? null : cboBloodType.Text;
            child.Address = txtAddress.Text;
            child.GuardianID = (int)cboGuardian.SelectedValue;
            return child;
        }

        private ChildSearchType GetSelectedSearchType()
        {
            return cboSearchBy.SelectedIndex switch
            {
                0 => ChildSearchType.ChildID,
                2 => ChildSearchType.GuardianName,
                _ => ChildSearchType.ChildName
            };
        }

        private string GetGuardianName(int guardianId)
        {
            Guardian? guardian = _guardians.FirstOrDefault(g => g.GuardianID == guardianId);
            return guardian == null ? "?" : guardian.FullName;
        }

        private void SetColumn(string columnName, string headerText, float fillWeight)
        {
            DataGridViewColumn? column = dgvChildren.Columns[columnName];
            if (column == null)
                return;

            column.HeaderText = headerText;
            column.FillWeight = fillWeight;
        }

        private void ClearInputs()
        {
            _selectedChildId = 0;
            txtFirstName.Clear();
            txtMiddleName.Clear();
            txtLastName.Clear();
            dtpDateOfBirth.Value = DateTime.Today;
            cboSex.SelectedIndex = -1;
            cboBloodType.SelectedIndex = 0;
            cboGuardian.SelectedIndex = -1;
            txtAddress.Clear();
            SetEditMode(false);
            txtFirstName.Focus();
        }

        // Add is only for NEW children; Update/Delete only for a selected one.
        private void SetEditMode(bool childSelected)
        {
            btnAdd.Enabled = !childSelected;
            btnUpdate.Enabled = childSelected;
            btnDelete.Enabled = childSelected;
        }

        private static void ShowError(string message, Exception ex)
        {
            MessageBox.Show(message + "\n\nDetails: " + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}