// Folder: Forms/HealthWorkersForm.cs
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
    /// Health worker management screen. PRESENTATION LAYER only: reads the controls,
    /// calls HealthWorkerService, shows results. Hashing and rules live in the service.
    /// </summary>
    public partial class HealthWorkersForm : Form
    {
        private readonly HealthWorkerService _service = new HealthWorkerService();
        private List<HealthWorker> _workers = new List<HealthWorker>();
        private int _selectedWorkerId = 0;   // 0 = nothing selected (adding a new worker)
        private bool _isLoading = false;     // stops the grid selection event while reloading

        public HealthWorkersForm()
        {
            InitializeComponent();

            // Suggestions only: the Position box is editable, so any text can be typed.
            cboPosition.Items.AddRange(new object[]
            {
                "Barangay Health Worker", "Barangay Nutrition Scholar",
                "Midwife", "Nurse", "Barangay Health Officer"
            });

            Load += HealthWorkersForm_Load;
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            btnResetPassword.Click += btnResetPassword_Click;
            btnSearch.Click += btnSearch_Click;
            btnShowAll.Click += btnShowAll_Click;
            dgvWorkers.SelectionChanged += dgvWorkers_SelectionChanged;
            txtSearch.KeyDown += txtSearch_KeyDown;
        }

        // ---------- form events ----------

        private void HealthWorkersForm_Load(object? sender, EventArgs e)
        {
            LoadWorkers(string.Empty);
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            try
            {
                HealthWorker worker = BuildWorkerFromInputs();   // validates the profile fields

                if (txtPassword.Text != txtConfirmPassword.Text)
                    throw new ArgumentException("Password and Confirm Password do not match.");

                _service.AddHealthWorker(worker, txtPassword.Text);

                MessageBox.Show("Health worker added successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtSearch.Clear();
                LoadWorkers(string.Empty);
            }
            catch (ArgumentException ex)          // invalid input
            {
                MessageBox.Show(ex.Message, "Please check your input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)  // business rule, e.g. username taken
            {
                MessageBox.Show(ex.Message, "Cannot save",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("The health worker could not be added.", ex);
            }
        }

        private void btnUpdate_Click(object? sender, EventArgs e)
        {
            if (_selectedWorkerId == 0)
            {
                MessageBox.Show("Click a health worker in the list first.", "Nothing selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                HealthWorker worker = BuildWorkerFromInputs();
                _service.UpdateHealthWorker(worker);   // the password is not changed here

                MessageBox.Show("Health worker updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadWorkers(txtSearch.Text.Trim());
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
                ShowError("The health worker could not be updated.", ex);
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (_selectedWorkerId == 0)
            {
                MessageBox.Show("Click a health worker in the list first.", "Nothing selected",
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
                _service.DeleteHealthWorker(_selectedWorkerId);

                MessageBox.Show("Health worker deleted.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadWorkers(txtSearch.Text.Trim());
            }
            catch (InvalidOperationException ex)   // e.g. has recorded vaccinations / last account
            {
                MessageBox.Show(ex.Message, "Cannot delete",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("The health worker could not be deleted.", ex);
            }
        }

        private void btnResetPassword_Click(object? sender, EventArgs e)
        {
            if (_selectedWorkerId == 0)
            {
                MessageBox.Show("Click a health worker in the list first.", "Nothing selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                if (txtPassword.Text != txtConfirmPassword.Text)
                    throw new ArgumentException("Password and Confirm Password do not match.");

                _service.ResetPassword(_selectedWorkerId, txtPassword.Text);

                MessageBox.Show("Password was reset successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtPassword.Clear();
                txtConfirmPassword.Clear();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Please check your input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("The password could not be reset.", ex);
            }
        }

        private void btnClear_Click(object? sender, EventArgs e)
        {
            dgvWorkers.ClearSelection();
            ClearInputs();
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            LoadWorkers(txtSearch.Text.Trim());
        }

        private void btnShowAll_Click(object? sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadWorkers(string.Empty);
        }

        private void txtSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoadWorkers(txtSearch.Text.Trim());
            }
        }

        // When a row is clicked, copy that worker into the controls for editing.
        private void dgvWorkers_SelectionChanged(object? sender, EventArgs e)
        {
            if (_isLoading || dgvWorkers.SelectedRows.Count == 0)
                return;

            int id = (int)dgvWorkers.SelectedRows[0].Cells["ID"].Value;
            HealthWorker? worker = _workers.FirstOrDefault(w => w.HealthWorkerID == id);
            if (worker == null)
                return;

            _selectedWorkerId = worker.HealthWorkerID;
            txtFirstName.Text = worker.FirstName;
            txtMiddleName.Text = worker.MiddleName ?? string.Empty;
            txtLastName.Text = worker.LastName;
            txtContactNumber.Text = worker.ContactNumber;
            cboPosition.Text = worker.Position;
            txtLicenseNo.Text = worker.LicenseNo ?? string.Empty;
            txtUsername.Text = worker.Username;
            txtPassword.Clear();
            txtConfirmPassword.Clear();

            SetEditMode(true);
        }

        // ---------- helper methods ----------

        private void LoadWorkers(string searchText)
        {
            try
            {
                _isLoading = true;
                _workers = _service.SearchHealthWorkers(searchText);

                dgvWorkers.DataSource = _workers.Select(w => new
                {
                    ID = w.HealthWorkerID,
                    Name = w.FullName,
                    Position = w.Position,
                    Contact = w.ContactNumber,
                    License = w.LicenseNo ?? "-",
                    Username = w.Username,
                    Role = w.Role
                }).ToList();

                DataGridViewColumn? idColumn = dgvWorkers.Columns["ID"];
                if (idColumn != null)
                    idColumn.FillWeight = 30;

                dgvWorkers.ClearSelection();
                lblCount.Text = _workers.Count == 1
                    ? "1 health worker shown"
                    : $"{_workers.Count} health workers shown";
            }
            catch (Exception ex)
            {
                ShowError("The health worker list could not be loaded.", ex);
            }
            finally
            {
                _isLoading = false;
            }

            ClearInputs();
        }

        // Creates a HealthWorker object from the controls. Each assignment runs the
        // validation inside the HealthWorker class (bad input throws ArgumentException).
        private HealthWorker BuildWorkerFromInputs()
        {
            HealthWorker worker = new HealthWorker();
            worker.HealthWorkerID = _selectedWorkerId;
            worker.FirstName = txtFirstName.Text;
            worker.MiddleName = txtMiddleName.Text;
            worker.LastName = txtLastName.Text;
            worker.ContactNumber = txtContactNumber.Text;
            worker.Position = cboPosition.Text;
            worker.LicenseNo = txtLicenseNo.Text;
            worker.Username = txtUsername.Text;
            return worker;
        }

        private void ClearInputs()
        {
            _selectedWorkerId = 0;
            txtFirstName.Clear();
            txtMiddleName.Clear();
            txtLastName.Clear();
            txtContactNumber.Clear();
            cboPosition.SelectedIndex = -1;
            cboPosition.Text = string.Empty;
            txtLicenseNo.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            SetEditMode(false);
            txtFirstName.Focus();
        }

        // Add is only for NEW workers; Update/Delete/Reset only for a selected one.
        private void SetEditMode(bool workerSelected)
        {
            btnAdd.Enabled = !workerSelected;
            btnUpdate.Enabled = workerSelected;
            btnDelete.Enabled = workerSelected;
            btnResetPassword.Enabled = workerSelected;
        }

        private static void ShowError(string message, Exception ex)
        {
            MessageBox.Show(message + "\n\nDetails: " + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}