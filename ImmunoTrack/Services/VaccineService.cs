// Folder: Services/VaccineService.cs
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ImmunoTrack.DataAccess;
using ImmunoTrack.Models;

namespace ImmunoTrack.Services
{
    /// <summary>
    /// Business layer for vaccines and their dose schedules.
    /// The schedule values are configurable data: the system never invents
    /// medical intervals, it only uses what is entered here.
    /// </summary>
    public class VaccineService
    {
        private readonly VaccineDataAccess _dataAccess = new VaccineDataAccess();

        // ---------- vaccines ----------

        public List<Vaccine> GetAllVaccines()
        {
            return _dataAccess.GetAllVaccines();
        }

        public int AddVaccine(Vaccine vaccine)
        {
            if (_dataAccess.VaccineNameExists(vaccine.VaccineName, 0))
                throw new InvalidOperationException("A vaccine with that name already exists.");

            return _dataAccess.AddVaccine(vaccine);
        }

        public void UpdateVaccine(Vaccine vaccine)
        {
            if (vaccine.VaccineID <= 0)
                throw new InvalidOperationException("Select a vaccine to update first.");

            if (_dataAccess.VaccineNameExists(vaccine.VaccineName, vaccine.VaccineID))
                throw new InvalidOperationException("Another vaccine already has that name.");

            _dataAccess.UpdateVaccine(vaccine);
        }

        public void DeleteVaccine(int vaccineId)
        {
            if (vaccineId <= 0)
                throw new InvalidOperationException("Select a vaccine to delete first.");

            try
            {
                _dataAccess.DeleteVaccine(vaccineId);
            }
            catch (SqlException ex) when (ex.Number == 547)   // foreign key conflict
            {
                throw new InvalidOperationException(
                    "This vaccine still has dose schedules, so it cannot be deleted. " +
                    "Delete its doses first, or untick Active to stop using it.");
            }
        }

        // ---------- dose schedule ----------

        public List<VaccineSchedule> GetSchedules(int vaccineId)
        {
            return _dataAccess.GetSchedulesByVaccine(vaccineId);
        }

        public int AddSchedule(VaccineSchedule schedule)
        {
            if (schedule.VaccineID <= 0)
                throw new InvalidOperationException("Select a vaccine first.");

            ValidateSchedule(schedule);

            if (_dataAccess.DoseExists(schedule.VaccineID, schedule.DoseNumber))
                throw new InvalidOperationException(
                    "Dose " + schedule.DoseNumber + " already exists for this vaccine.");

            return _dataAccess.AddSchedule(schedule);
        }

        public void UpdateSchedule(VaccineSchedule schedule)
        {
            if (schedule.ScheduleID <= 0)
                throw new InvalidOperationException("Select a dose to update first.");

            ValidateSchedule(schedule);
            _dataAccess.UpdateSchedule(schedule);
        }

        public void DeleteSchedule(int scheduleId)
        {
            if (scheduleId <= 0)
                throw new InvalidOperationException("Select a dose to delete first.");

            try
            {
                _dataAccess.DeleteSchedule(scheduleId);
            }
            catch (SqlException ex) when (ex.Number == 547)   // foreign key conflict
            {
                throw new InvalidOperationException(
                    "This dose has already been recorded for one or more children, so it cannot be deleted. " +
                    "Untick Active to stop using it instead.");
            }
        }

        // Business rule: dose 1 has no previous dose, so it cannot have an interval.
        private static void ValidateSchedule(VaccineSchedule schedule)
        {
            if (schedule.DoseNumber == 1 && schedule.MinimumIntervalDays.HasValue)
                throw new ArgumentException(
                    "Dose 1 has no previous dose, so leave the minimum interval blank.");
        }
    }
}
