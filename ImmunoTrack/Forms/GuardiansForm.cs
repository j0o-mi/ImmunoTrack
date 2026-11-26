// Folder: Forms/GuardiansForm.cs
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
    /// Guardian management screen. PRESENTATION LAYER only: it reads the
    /// textboxes, calls GuardianService, and shows the results. There is
    /// no SQL and no business rule in this file.
    /// </summary>
    public partial class GuardiansForm : Form
    {
        private readonly GuardianService _service = new GuardianService();
        private List<Guardian> _guardians = new List<Guardian>();
        private int _selectedGuardianId = 0;   // 0 = nothing selected (adding a new guardian)
        private bool _isLoading = false;       // stops the grid selection event while reloading

        public GuardiansForm()
        {
            InitializeComponent();

            cboRelationship.Items.AddRange(new object[]
            {
                "Mother", "Father", "Grandmother", "Grandfather",
                "Aunt", "Uncle", "Sibling", "Legal Guardian", "Other"
            });

            // Connect every control to its method (no double-clicking needed).
            Load += GuardiansForm_Load;
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            btnSearch.Click += btnSearch_Click;
            btnShowAll.Click += btnShowAll_Click;
            dgvGuardians.SelectionChanged += dgvGuardians_SelectionChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
        }

        // ---------- form events ----------

        private void GuardiansForm_Load(object? sender, EventArgs e)
        {
            LoadGuardians(string.Empty);
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            try
            {
                Guardian guardian = BuildGuardianFromInputs();
                _service.AddGuardian(guardian);

                MessageBox.Show("Guardian added successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtSearch.Clear();
                LoadGuardians(string.Empty);
            }
            catch (ArgumentException ex)    // thrown by the Guardian class when input is invalid
            {
                MessageBox.Show(ex.Message, "Please check your input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("The guardian could not be added.", ex);
            }
        }

        private void btnUpdate_Click(object? sender, EventArgs e)
        {
            if (_selectedGuardianId == 0)
            {
                MessageBox.Show("Click a guardian in the list first.", "Nothing selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                Guardian guardian = BuildGuardianFromInputs();
                _service.UpdateGuardian(guardian);

                MessageBox.Show("Guardian updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadGuardians(txtSearch.Text.Trim());
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Please check your input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("The guardian could not be updated.", ex);
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedGuardianId == 0)
            {
                MessageBox.Show("Click a guardian in the list first.", "Nothing selected",
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
                _service.DeleteGuardian(_selectedGuardianId);

                MessageBox.Show("Guardian deleted.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadGuardians(txtSearch.Text.Trim());
            }
            catch (InvalidOperationException ex)   // e.g. the guardian still has children
            {
                MessageBox.Show(ex.Message, "Cannot delete",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("The guardian could not be deleted.", ex);
            }
        }

        private void btnClear_Click(object? sender, EventArgs e)
        {
            dgvGuardians.ClearSelection();
            ClearInputs();
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            LoadGuardians(txtSearch.Text.Trim());
        }

        private void btnShowAll_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadGuardians(string.Empty);
        }

        private void txtSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;   // stops the "ding" sound
                LoadGuardians(txtSearch.Text.Trim());
            }
        }

        // When a row is clicked, copy that guardian into the textboxes for editing.
        private void dgvGuardians_SelectionChanged(object? sender, EventArgs e)
        {
            if (_isLoading || dgvGuardians.SelectedRows.Count == 0)
                return;

            int id = (int)dgvGuardians.SelectedRows[0].Cells["ID"].Value;
            Guardian? guardian = _guardians.FirstOrDefault(g => g.GuardianID == id);
            if (guardian == null)
                return;

            _selectedGuardianId = guardian.GuardianID;
            txtFirstName.Text = guardian.FirstName;
            txtMiddleName.Text = guardian.MiddleName ?? string.Empty;
            txtLastName.Text = guardian.LastName;
            txtContactNumber.Text = guardian.ContactNumber;
            txtAddress.Text = guardian.Address;
            cboRelationship.SelectedIndex = cboRelationship.FindStringExact(guardian.RelationshipToChild);

            SetEditMode(true);
        }

        // ---------- helper methods ----------

        // Loads guardians from the service (all, or filtered by search text) into the grid.
        private void LoadGuardians(string searchText)
        {
            try
            {
                _isLoading = true;
                _guardians = _service.SearchGuardians(searchText);

                // Show only the columns a health worker needs.
                dgvGuardians.DataSource = _guardians.Select(g => new
                {
                    ID = g.GuardianID,
                    Name = g.FullName,
                    Contact = g.ContactNumber,
                    Relationship = g.RelationshipToChild,
                    Address = g.Address
                }).ToList();

                dgvGuardians.ClearSelection();
                lblCount.Text = _guardians.Count == 1
                    ? "1 guardian shown"
                    : $"{_guardians.Count} guardians shown";
            }
            catch (Exception ex)
            {
                ShowError("The guardian list could not be loaded.", ex);
            }
            finally
            {
                _isLoading = false;
            }

            ClearInputs();
        }

        // Creates a Guardian object from the textboxes. Each assignment runs the
        // validation inside the Guardian class, so bad input throws an ArgumentException.
        private Guardian BuildGuardianFromInputs()
        {
            Guardian guardian = new Guardian();
            guardian.GuardianID = _selectedGuardianId;
            guardian.FirstName = txtFirstName.Text;
            guardian.MiddleName = txtMiddleName.Text;
            guardian.LastName = txtLastName.Text;
            guardian.ContactNumber = txtContactNumber.Text;
            guardian.RelationshipToChild = cboRelationship.Text;
            guardian.Address = txtAddress.Text;
            return guardian;
        }

        private void ClearInputs()
        {
            _selectedGuardianId = 0;
            txtFirstName.Clear();
            txtMiddleName.Clear();
            txtLastName.Clear();
            txtContactNumber.Clear();
            txtAddress.Clear();
            cboRelationship.SelectedIndex = -1;
            SetEditMode(false);
            txtFirstName.Focus();
        }

        // Add is only for NEW guardians; Update/Delete only for a selected one.
        private void SetEditMode(bool guardianSelected)
        {
            btnAdd.Enabled = !guardianSelected;
            btnUpdate.Enabled = guardianSelected;
            btnDelete.Enabled = guardianSelected;
        }

        private static void ShowError(string message, Exception ex)
        {
            MessageBox.Show(message + "\n\nDetails: " + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}