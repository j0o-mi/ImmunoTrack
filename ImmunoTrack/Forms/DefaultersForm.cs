// Folder: Forms/DefaultersForm.cs
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
    /// Defaulter monitoring screen. PRESENTATION LAYER only: DefaulterService works
    /// out every child's status; this form shows it and applies the filter the
    /// health worker picks.
    /// </summary>
    public partial class DefaultersForm : Form
    {
        private readonly DefaulterService _service = new DefaulterService();
        private List<ChildStatusInfo> _all = new List<ChildStatusInfo>();   // every child, loaded once

        public DefaultersForm()
        {
            InitializeComponent();

            // Same order as GetSelectedStatus() below.
            cboStatus.Items.AddRange(new object[]
            {
                "Overdue (Defaulters)", "Due Today", "Upcoming", "Fully Vaccinated", "All Children"
            });
            cboStatus.SelectedIndex = 0;     // start by showing the defaulters

            Load += DefaultersForm_Load;
            cboStatus.SelectedIndexChanged += FilterChanged;
            txtSearch.TextChanged += FilterChanged;
            btnRefresh.Click += btnRefresh_Click;
        }

        private void DefaultersForm_Load(object? sender, EventArgs e)
        {
            LoadData();
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            LoadData();
        }

        // Changing the filter or search text only re-filters the loaded list (no database call).
        private void FilterChanged(object? sender, EventArgs e)
        {
            ApplyFilter();
        }

        // Loads every child's status from the database and updates the summary cards.
        private void LoadData()
        {
            try
            {
                _all = _service.GetChildStatuses();

                StatusCounts counts = _service.CountByStatus(_all);
                lblTotalCount.Text = counts.Total.ToString();
                lblOverdueCount.Text = counts.Overdue.ToString();
                lblDueTodayCount.Text = counts.DueToday.ToString();
                lblUpcomingCount.Text = counts.Upcoming.ToString();
                lblFullyCount.Text = counts.FullyVaccinated.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("The defaulter list could not be loaded.\n\nDetails: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _all = new List<ChildStatusInfo>();
            }

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            List<ChildStatusInfo> shown = _service.Filter(_all, GetSelectedStatus(), txtSearch.Text);

            dgvChildren.DataSource = shown.Select(i => new
            {
                ID = i.Child.ChildID,
                Child = i.Child.FullName,
                Age = i.Child.GetAgeText(),
                Guardian = i.GuardianName,
                Contact = i.GuardianContact,
                Status = i.OverallStatus.ToDisplayText(),
                NextVaccine = i.NextDose == null ? "-" : i.NextDose.VaccineName,
                Dose = i.NextDose == null ? "-" : i.NextDose.DoseNumber.ToString(),
                DueDate = i.NextDose == null ? "-" : i.NextDose.DueDate.ToString("yyyy-MM-dd"),
                DaysOverdue = i.DaysOverdue > 0 ? i.DaysOverdue.ToString() : "-",
                MissedDoses = i.OverdueDoseCount
            }).ToList();

            SetColumn("ID", "ID", 25);
            SetColumn("Age", "Age", 60);
            SetColumn("NextVaccine", "Next Vaccine", 70);
            SetColumn("Dose", "Dose", 30);
            SetColumn("DueDate", "Due Date", 60);
            SetColumn("DaysOverdue", "Days Late", 45);
            SetColumn("MissedDoses", "Missed", 40);

            // Colour each status so overdue children stand out.
            for (int i = 0; i < shown.Count; i++)
                dgvChildren.Rows[i].Cells["Status"].Style.ForeColor = GetStatusColor(shown[i].OverallStatus);

            dgvChildren.ClearSelection();
            lblCount.Text = shown.Count == 1 ? "1 child shown" : shown.Count + " children shown";
        }

        // The dropdown choice as a status (null = show every child).
        private VaccinationStatus? GetSelectedStatus()
        {
            switch (cboStatus.SelectedIndex)
            {
                case 0:
                    return VaccinationStatus.Overdue;
                case 1:
                    return VaccinationStatus.DueToday;
                case 2:
                    return VaccinationStatus.Upcoming;
                case 3:
                    return VaccinationStatus.FullyVaccinated;
                default:
                    return null;
            }
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

        private void SetColumn(string columnName, string headerText, float fillWeight)
        {
            DataGridViewColumn? column = dgvChildren.Columns[columnName];
            if (column == null)
                return;

            column.HeaderText = headerText;
            column.FillWeight = fillWeight;
        }

        private void DefaultersForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}