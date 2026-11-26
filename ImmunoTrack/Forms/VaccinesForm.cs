// Folder: Forms/VaccinesForm.cs
// (This is the behavior file.)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ImmunoTrack.Models;
using ImmunoTrack.Services;

namespace ImmunoTrack.Forms
{
    /// <summary>
    /// Vaccine list (top) and the dose schedule of the selected vaccine (bottom).
    /// PRESENTATION LAYER only: it reads the controls and calls VaccineService.
    /// </summary>
    public partial class VaccinesForm : Form
    {
        private readonly VaccineService _service = new VaccineService();

        private List<Vaccine> _vaccines = new List<Vaccine>();
        private List<VaccineSchedule> _schedules = new List<VaccineSchedule>();
        private int _selectedVaccineId = 0;    // 0 = no vaccine selected
        private int _selectedScheduleId = 0;   // 0 = adding a new dose
        private bool _isLoadingVaccines = false;
        private bool _isLoadingSchedules = false;

        public VaccinesForm()
        {
            InitializeComponent();

            Load += VaccinesForm_Load;
            btnAddVaccine.Click += btnAddVaccine_Click;
            btnUpdateVaccine.Click += btnUpdateVaccine_Click;
            btnDeleteVaccine.Click += btnDeleteVaccine_Click;
            btnClearVaccine.Click += btnClearVaccine_Click;
            dgvVaccines.SelectionChanged += dgvVaccines_SelectionChanged;

            btnAddDose.Click += btnAddDose_Click;
            btnUpdateDose.Click += btnUpdateDose_Click;
            btnDeleteDose.Click += btnDeleteDose_Click;
            btnClearDose.Click += btnClearDose_Click;
            dgvSchedule.SelectionChanged += dgvSchedule_SelectionChanged;
        }

        private void VaccinesForm_Load(object? sender, EventArgs e)
        {
            LoadVaccines();
        }

        // =====================  VACCINE EVENTS  =====================

        private void btnAddVaccine_Click(object? sender, EventArgs e)
        {
            try
            {
                Vaccine vaccine = BuildVaccineFromInputs();
                _service.AddVaccine(vaccine);

                MessageBox.Show("Vaccine added successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadVaccines();
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
                ShowError("The vaccine could not be added.", ex);
            }
        }

        private void btnUpdateVaccine_Click(object? sender, EventArgs e)
        {
            if (_selectedVaccineId == 0)
            {
                ShowNothingSelected("Click a vaccine in the list first.");
                return;
            }

            try
            {
                Vaccine vaccine = BuildVaccineFromInputs();
                _service.UpdateVaccine(vaccine);

                MessageBox.Show("Vaccine updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadVaccines();
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
                ShowError("The vaccine could not be updated.", ex);
            }
        }

        private void btnDeleteVaccine_Click(object? sender, EventArgs e)
        {
            if (_selectedVaccineId == 0)
            {
                ShowNothingSelected("Click a vaccine in the list first.");
                return;
            }

            DialogResult answer = MessageBox.Show(
                $"Delete {txtVaccineName.Text}? This cannot be undone.", "Confirm delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (answer != DialogResult.Yes)
                return;

            try
            {
                _service.DeleteVaccine(_selectedVaccineId);

                MessageBox.Show("Vaccine deleted.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadVaccines();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Cannot delete",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("The vaccine could not be deleted.", ex);
            }
        }

        private void btnClearVaccine_Click(object? sender, EventArgs e)
        {
            dgvVaccines.ClearSelection();
            ClearVaccineInputs();
        }

        // Clicking a vaccine fills its fields and shows ITS doses below.
        private void dgvVaccines_SelectionChanged(object? sender, EventArgs e)
        {
            if (_isLoadingVaccines || dgvVaccines.SelectedRows.Count == 0)
                return;

            int id = (int)dgvVaccines.SelectedRows[0].Cells["ID"].Value;
            Vaccine? vaccine = _vaccines.FirstOrDefault(v => v.VaccineID == id);
            if (vaccine == null)
                return;

            _selectedVaccineId = vaccine.VaccineID;
            txtVaccineName.Text = vaccine.VaccineName;
            txtDescription.Text = vaccine.Description ?? string.Empty;
            chkVaccineActive.Checked = vaccine.IsActive;
            SetVaccineEditMode(true);

            LoadSchedules();
        }

        // =====================  DOSE EVENTS  =====================

        private void btnAddDose_Click(object? sender, EventArgs e)
        {
            try
            {
                VaccineSchedule schedule = BuildScheduleFromInputs();
                _service.AddSchedule(schedule);

                MessageBox.Show("Dose added successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadSchedules();
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
                ShowError("The dose could not be added.", ex);
            }
        }

        private void btnUpdateDose_Click(object? sender, EventArgs e)
        {
            if (_selectedScheduleId == 0)
            {
                ShowNothingSelected("Click a dose in the schedule list first.");
                return;
            }

            try
            {
                VaccineSchedule schedule = BuildScheduleFromInputs();
                _service.UpdateSchedule(schedule);

                MessageBox.Show("Dose updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadSchedules();
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
                ShowError("The dose could not be updated.", ex);
            }
        }

        private void btnDeleteDose_Click(object? sender, EventArgs e)
        {
            if (_selectedScheduleId == 0)
            {
                ShowNothingSelected("Click a dose in the schedule list first.");
                return;
            }

            DialogResult answer = MessageBox.Show(
                $"Delete dose {nudDoseNumber.Value}? This cannot be undone.", "Confirm delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (answer != DialogResult.Yes)
                return;

            try
            {
                _service.DeleteSchedule(_selectedScheduleId);

                MessageBox.Show("Dose deleted.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadSchedules();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Cannot delete",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("The dose could not be deleted.", ex);
            }
        }

        private void btnClearDose_Click(object? sender, EventArgs e)
        {
            dgvSchedule.ClearSelection();
            ClearScheduleInputs();
        }

        private void dgvSchedule_SelectionChanged(object? sender, EventArgs e)
        {
            if (_isLoadingSchedules || dgvSchedule.SelectedRows.Count == 0)
                return;

            int id = (int)dgvSchedule.SelectedRows[0].Cells["ID"].Value;
            VaccineSchedule? schedule = _schedules.FirstOrDefault(s => s.ScheduleID == id);
            if (schedule == null)
                return;

            _selectedScheduleId = schedule.ScheduleID;
            nudDoseNumber.Value = Math.Min(schedule.DoseNumber, (int)nudDoseNumber.Maximum);
            nudDoseNumber.Enabled = false;    // the dose number is fixed once created
            nudAgeDays.Value = Math.Min(schedule.RecommendedAgeDays, (int)nudAgeDays.Maximum);
            txtInterval.Text = schedule.MinimumIntervalDays?.ToString() ?? string.Empty;
            chkScheduleActive.Checked = schedule.IsActive;

            SetScheduleEditMode(true);
        }

        // =====================  LOADING  =====================

        private void LoadVaccines()
        {
            try
            {
                _isLoadingVaccines = true;
                _vaccines = _service.GetAllVaccines();

                dgvVaccines.DataSource = _vaccines.Select(v => new
                {
                    ID = v.VaccineID,
                    Name = v.VaccineName,
                    Description = v.Description ?? "-",
                    Active = v.IsActive ? "Yes" : "No"
                }).ToList();

                SetColumn(dgvVaccines, "ID", "ID", 25);
                SetColumn(dgvVaccines, "Active", "Active", 30);
                dgvVaccines.ClearSelection();
            }
            catch (Exception ex)
            {
                ShowError("The vaccine list could not be loaded.", ex);
            }
            finally
            {
                _isLoadingVaccines = false;
            }

            ClearVaccineInputs();
        }

        // Loads the doses of the selected vaccine (or shows nothing if none is selected).
        private void LoadSchedules()
        {
            try
            {
                _isLoadingSchedules = true;

                if (_selectedVaccineId == 0)
                {
                    _schedules = new List<VaccineSchedule>();
                    dgvSchedule.DataSource = null;
                    grpSchedule.Text = "Dose Schedule (select a vaccine above first)";
                    grpSchedule.Enabled = false;
                }
                else
                {
                    _schedules = _service.GetSchedules(_selectedVaccineId);

                    dgvSchedule.DataSource = _schedules.Select(s => new
                    {
                        ID = s.ScheduleID,
                        Dose = s.DoseNumber,
                        AgeDays = s.RecommendedAgeDays,
                        IntervalDays = s.MinimumIntervalDays.HasValue
                            ? s.MinimumIntervalDays.Value.ToString()
                            : "-",
                        Active = s.IsActive ? "Yes" : "No"
                    }).ToList();

                    SetColumn(dgvSchedule, "ID", "ID", 25);
                    SetColumn(dgvSchedule, "Dose", "Dose", 30);
                    SetColumn(dgvSchedule, "AgeDays", "Recommended Age (days)", 60);
                    SetColumn(dgvSchedule, "IntervalDays", "Min. Interval (days)", 60);
                    SetColumn(dgvSchedule, "Active", "Active", 30);

                    grpSchedule.Text = "Dose Schedule for: " + txtVaccineName.Text;
                    grpSchedule.Enabled = true;
                }

                dgvSchedule.ClearSelection();
            }
            catch (Exception ex)
            {
                ShowError("The dose schedule could not be loaded.", ex);
            }
            finally
            {
                _isLoadingSchedules = false;
            }

            ClearScheduleInputs();
        }

        // =====================  HELPERS  =====================

        private Vaccine BuildVaccineFromInputs()
        {
            Vaccine vaccine = new Vaccine();
            vaccine.VaccineID = _selectedVaccineId;
            vaccine.VaccineName = txtVaccineName.Text;
            vaccine.Description = txtDescription.Text;
            vaccine.IsActive = chkVaccineActive.Checked;
            return vaccine;
        }

        private VaccineSchedule BuildScheduleFromInputs()
        {
            VaccineSchedule schedule = new VaccineSchedule();
            schedule.ScheduleID = _selectedScheduleId;
            schedule.VaccineID = _selectedVaccineId;
            schedule.DoseNumber = (int)nudDoseNumber.Value;
            schedule.RecommendedAgeDays = (int)nudAgeDays.Value;
            schedule.MinimumIntervalDays = ParseInterval();
            schedule.IsActive = chkScheduleActive.Checked;
            return schedule;
        }

        // Blank = no interval (null); otherwise it must be a whole number of days.
        private int? ParseInterval()
        {
            string text = txtInterval.Text.Trim();
            if (text.Length == 0)
                return null;

            if (!int.TryParse(text, out int days))
                throw new ArgumentException("Minimum interval must be a whole number of days (or left blank).");

            return days;
        }

        private void ClearVaccineInputs()
        {
            _selectedVaccineId = 0;
            txtVaccineName.Clear();
            txtDescription.Clear();
            chkVaccineActive.Checked = true;
            SetVaccineEditMode(false);
            LoadSchedules();               // with no vaccine selected this disables the dose section
            txtVaccineName.Focus();
        }

        private void ClearScheduleInputs()
        {
            _selectedScheduleId = 0;

            // Suggest the next dose number (highest existing + 1).
            int nextDose = _schedules.Count == 0 ? 1 : _schedules.Max(s => s.DoseNumber) + 1;
            nudDoseNumber.Value = Math.Min(nextDose, (int)nudDoseNumber.Maximum);
            nudDoseNumber.Enabled = true;

            nudAgeDays.Value = 0;
            txtInterval.Clear();
            chkScheduleActive.Checked = true;
            SetScheduleEditMode(false);
        }

        private void SetVaccineEditMode(bool vaccineSelected)
        {
            btnAddVaccine.Enabled = !vaccineSelected;
            btnUpdateVaccine.Enabled = vaccineSelected;
            btnDeleteVaccine.Enabled = vaccineSelected;
        }

        private void SetScheduleEditMode(bool scheduleSelected)
        {
            btnAddDose.Enabled = !scheduleSelected;
            btnUpdateDose.Enabled = scheduleSelected;
            btnDeleteDose.Enabled = scheduleSelected;
        }

        private static void SetColumn(DataGridView grid, string columnName, string headerText, float fillWeight)
        {
            DataGridViewColumn? column = grid.Columns[columnName];
            if (column == null)
                return;

            column.HeaderText = headerText;
            column.FillWeight = fillWeight;
        }

        private static void ShowNothingSelected(string message)
        {
            MessageBox.Show(message, "Nothing selected",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static void ShowError(string message, Exception ex)
        {
            MessageBox.Show(message + "\n\nDetails: " + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}