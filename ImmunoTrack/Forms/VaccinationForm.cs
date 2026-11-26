// Folder: Forms/VaccinationForm.cs
// (This is the behavior file.)
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ImmunoTrack.Models;
using ImmunoTrack.Services;

namespace ImmunoTrack.Forms
{
    /// <summary>
    /// The vaccination transaction screen. PRESENTATION LAYER only: it shows what
    /// VaccinationService calculates and sends back what the health worker enters.
    /// The due-date rules are in ImmunizationCalculator, not here.
    /// </summary>
    public partial class VaccinationForm : Form
    {
        private readonly ChildService _childService = new ChildService();
        private readonly GuardianService _guardianService = new GuardianService();
        private readonly VaccinationService _vaccinationService = new VaccinationService();

        private List<Child> _children = new List<Child>();
        private int _selectedChildId = 0;          // 0 = no child selected
        private bool _isLoadingChildren = false;   // stops the dropdown event while it is refilled

        public VaccinationForm()
        {
            InitializeComponent();

            dtpDate.MaxDate = DateTime.Today;      // a dose cannot be dated in the future
            dtpDate.Value = DateTime.Today;

            Load += VaccinationForm_Load;
            txtFindChild.TextChanged += txtFindChild_TextChanged;
            cboChild.SelectedIndexChanged += cboChild_SelectedIndexChanged;
            btnRecord.Click += btnRecord_Click;
        }

        // ---------- form events ----------

        private void VaccinationForm_Load(object? sender, EventArgs e)
        {
            // The logged-in health worker is automatically the one who gives the dose.
            HealthWorker? user = UserSession.CurrentUser;
            txtWorker.Text = user == null ? "(not logged in)" : user.FullName;

            try
            {
                _children = _childService.GetAllChildren();
            }
            catch (Exception ex)
            {
                ShowError("The children list could not be loaded.", ex);
            }

            FilterChildren();
        }

        // Typing in the Find box narrows the child dropdown.
        private void txtFindChild_TextChanged(object? sender, EventArgs e)
        {
            FilterChildren();
        }

        private void cboChild_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_isLoadingChildren)
                return;

            if (cboChild.SelectedValue == null)
            {
                ClearChildDetails();
                return;
            }

            LoadChildSummary((int)cboChild.SelectedValue);
        }

        private void btnRecord_Click(object? sender, EventArgs e)
        {
            try
            {
                VaccinationRecord record = BuildRecordFromInputs();

                string doseText = cboDose.Text;
                string childName = cboChild.Text;
                DialogResult answer = MessageBox.Show(
                    $"Record this dose?\n\n{doseText}\nChild: {childName}\nDate: {record.DateAdministered:yyyy-MM-dd}",
                    "Confirm vaccination", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (answer != DialogResult.Yes)
                    return;

                _vaccinationService.RecordVaccination(record);

                MessageBox.Show("Vaccination recorded successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtRemarks.Clear();
                dtpDate.Value = DateTime.Today;
                LoadChildSummary(_selectedChildId);   // refresh history, next doses and status
            }
            catch (ArgumentException ex)          // invalid input, thrown by VaccinationRecord
            {
                MessageBox.Show(ex.Message, "Please check your input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)  // business rule, e.g. wrong dose order
            {
                MessageBox.Show(ex.Message, "Cannot record this dose",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ShowError("The vaccination could not be recorded.", ex);
            }
        }

        // ---------- loading ----------

        // Refills the child dropdown with the children matching the Find box.
        private void FilterChildren()
        {
            string text = txtFindChild.Text.Trim();

            List<Child> matches = _children;
            if (text.Length > 0)
            {
                matches = _children
                    .Where(c => c.FullName.Contains(text, StringComparison.OrdinalIgnoreCase)
                             || c.ChildID.ToString() == text)
                    .ToList();
            }

            _isLoadingChildren = true;
            cboChild.DisplayMember = "Label";
            cboChild.ValueMember = "ChildID";
            cboChild.DataSource = matches.Select(c => new
            {
                c.ChildID,
                Label = c.FullName + "  (ID " + c.ChildID + ")"
            }).ToList();
            cboChild.SelectedIndex = -1;
            _isLoadingChildren = false;

            if (matches.Count == 1)
                cboChild.SelectedIndex = 0;     // exactly one match: select it automatically
            else
                ClearChildDetails();
        }

        // Loads and shows everything about the chosen child.
        private void LoadChildSummary(int childId)
        {
            try
            {
                ChildImmunizationSummary summary = _vaccinationService.GetSummary(childId);
                _selectedChildId = childId;

                Guardian? guardian = _guardianService.GetGuardian(summary.Child.GuardianID);
                string guardianText = guardian == null
                    ? "Guardian: ?"
                    : "Guardian: " + guardian.FullName + " (" + guardian.RelationshipToChild + ")";

                lblChildInfo.Text = summary.Child.GetAgeText() + " old  |  " + summary.Child.Sex
                    + "\n" + guardianText;

                ShowSummary(summary);
                BindNextDoses(summary.NextDoses);
                BindHistory(summary.History);
                BindDoseDropdown(summary.NextDoses);
            }
            catch (Exception ex)
            {
                ShowError("The child's vaccination information could not be loaded.", ex);
                ClearChildDetails();
            }
        }

        // The summary block: latest dose, next required dose, and status.
        private void ShowSummary(ChildImmunizationSummary summary)
        {
            VaccinationHistoryItem? latest = summary.LatestRecord;
            lblLatest.Text = latest == null
                ? "Latest vaccine given:  none recorded yet"
                : $"Latest vaccine given:  {latest.VaccineName}, Dose {latest.DoseNumber}  on {latest.DateAdministered:yyyy-MM-dd}";

            NextDoseInfo? next = summary.NextRequired;
            lblNext.Text = next == null
                ? "Next required:  none, all doses are recorded"
                : $"Next required:  {next.VaccineName}, Dose {next.DoseNumber}  due {next.DueDate:yyyy-MM-dd}";

            lblStatus.Text = "Status:  " + (next == null
                ? VaccinationStatus.FullyVaccinated.ToDisplayText()
                : next.GetStatusText());
            lblStatus.ForeColor = GetStatusColor(summary.OverallStatus);
        }

        private void BindNextDoses(List<NextDoseInfo> nextDoses)
        {
            dgvNextDoses.DataSource = nextDoses.Select(n => new
            {
                Vaccine = n.VaccineName,
                Dose = n.DoseNumber,
                DueDate = n.DueDate.ToString("yyyy-MM-dd"),
                Status = n.GetStatusText()
            }).ToList();

            SetColumn(dgvNextDoses, "Dose", "Dose", 30);
            SetColumn(dgvNextDoses, "DueDate", "Due Date", 55);
            SetColumn(dgvNextDoses, "Status", "Status", 80);

            // Colour each status so overdue doses stand out.
            for (int i = 0; i < nextDoses.Count; i++)
                dgvNextDoses.Rows[i].Cells["Status"].Style.ForeColor = GetStatusColor(nextDoses[i].Status);

            dgvNextDoses.ClearSelection();
        }

        private void BindHistory(List<VaccinationHistoryItem> history)
        {
            dgvHistory.DataSource = history.Select(h => new
            {
                Date = h.DateAdministered.ToString("yyyy-MM-dd"),
                Vaccine = h.VaccineName,
                Dose = h.DoseNumber,
                AdministeredBy = h.AdministeredBy,
                Remarks = h.Remarks ?? "-"
            }).ToList();

            SetColumn(dgvHistory, "Date", "Date", 55);
            SetColumn(dgvHistory, "Dose", "Dose", 30);
            SetColumn(dgvHistory, "AdministeredBy", "Administered By", 70);
            dgvHistory.ClearSelection();
        }

        // Only the child's NEXT required doses can be recorded.
        private void BindDoseDropdown(List<NextDoseInfo> nextDoses)
        {
            cboDose.DisplayMember = "Label";
            cboDose.ValueMember = "ScheduleID";
            cboDose.DataSource = nextDoses.Select(n => new
            {
                n.ScheduleID,
                Label = n.VaccineName + " - Dose " + n.DoseNumber + "  (due " + n.DueDate.ToString("yyyy-MM-dd") + ")"
            }).ToList();

            bool hasDoses = nextDoses.Count > 0;
            if (hasDoses)
                cboDose.SelectedIndex = 0;      // the most urgent dose first

            grpRecord.Enabled = hasDoses;
            grpRecord.Text = hasDoses
                ? "Record a Dose"
                : "Record a Dose (no pending doses)";
        }

        // Back to the "no child selected" state.
        private void ClearChildDetails()
        {
            _selectedChildId = 0;
            lblChildInfo.Text = string.Empty;
            lblLatest.Text = "Select a child to see the summary.";
            lblNext.Text = string.Empty;
            lblStatus.Text = string.Empty;
            dgvNextDoses.DataSource = null;
            dgvHistory.DataSource = null;
            cboDose.DataSource = null;
            grpRecord.Enabled = false;
            grpRecord.Text = "Record a Dose (select a child first)";
        }

        // ---------- helpers ----------

        // Creates the record from the screen. Each assignment runs the validation
        // inside VaccinationRecord, so bad input throws an ArgumentException.
        private VaccinationRecord BuildRecordFromInputs()
        {
            if (_selectedChildId == 0)
                throw new ArgumentException("Select a child first.");

            HealthWorker? user = UserSession.CurrentUser;
            if (user == null)
                throw new ArgumentException("No health worker is logged in.");

            if (cboDose.SelectedValue == null)
                throw new ArgumentException("Select the dose to record.");

            VaccinationRecord record = new VaccinationRecord();
            record.ChildID = _selectedChildId;
            record.ScheduleID = (int)cboDose.SelectedValue;
            record.DateAdministered = dtpDate.Value;
            record.HealthWorkerID = user.HealthWorkerID;
            record.Remarks = txtRemarks.Text;
            return record;
        }

        private static Color GetStatusColor(VaccinationStatus status)
        {
            return status switch
            {
                VaccinationStatus.Overdue => Color.FromArgb(198, 40, 40),
                VaccinationStatus.DueToday => Color.FromArgb(230, 81, 0),
                VaccinationStatus.Upcoming => Color.FromArgb(0, 121, 107),
                _ => Color.FromArgb(27, 94, 32)
            };
        }

        private static void SetColumn(DataGridView grid, string columnName, string headerText, float fillWeight)
        {
            DataGridViewColumn? column = grid.Columns[columnName];
            if (column == null)
                return;

            column.HeaderText = headerText;
            column.FillWeight = fillWeight;
        }

        private static void ShowError(string message, Exception ex)
        {
            MessageBox.Show(message + "\n\nDetails: " + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void VaccinationForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}