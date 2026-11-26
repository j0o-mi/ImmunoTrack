// Folder: Services/VaccinationService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using ImmunoTrack.DataAccess;
using ImmunoTrack.Models;

namespace ImmunoTrack.Services;

/// <summary>
/// Business layer for vaccination transactions: loads a child's picture,
/// checks that a dose may be recorded, and saves it.
/// </summary>
public class VaccinationService
{
    private readonly VaccinationDataAccess _dataAccess = new VaccinationDataAccess();
    private readonly ChildDataAccess _childDataAccess = new ChildDataAccess();

    // History + next doses + status of one child.
    public ChildImmunizationSummary GetSummary(int childId)
    {
        Child? child = _childDataAccess.GetById(childId);
        if (child == null)
            throw new InvalidOperationException("That child could not be found.");

        List<ScheduledDose> activeDoses = _dataAccess.GetActiveScheduledDoses();
        List<VaccinationHistoryItem> history = _dataAccess.GetHistoryByChild(childId);

        return ImmunizationCalculator.BuildSummary(child, activeDoses, history);
    }

    // Saves a vaccination after checking every rule. Returns the new RecordID.
    public int RecordVaccination(VaccinationRecord record)
    {
        ChildImmunizationSummary summary = GetSummary(record.ChildID);

        // Rule: only the child's NEXT required dose of a vaccine can be recorded.
        // This blocks doses given twice, doses recorded out of order, and inactive doses.
        NextDoseInfo? dose = summary.NextDoses.FirstOrDefault(n => n.ScheduleID == record.ScheduleID);
        if (dose == null)
        {
            throw new InvalidOperationException(
                "That dose cannot be recorded. It may already be recorded, an earlier dose of the same " +
                "vaccine may still be missing, or the dose is no longer active.");
        }

        // Rule: a dose cannot be given before the child was born.
        if (record.DateAdministered.Date < summary.Child.DateOfBirth.Date)
        {
            throw new InvalidOperationException(
                "The date administered cannot be before the child's date of birth (" +
                summary.Child.DateOfBirth.ToString("yyyy-MM-dd") + ").");
        }

        // Rule: a dose cannot be dated before the previous dose of the same vaccine.
        VaccinationHistoryItem? previous = ImmunizationCalculator.FindPreviousRecord(
            summary.History, dose.VaccineID, dose.DoseNumber);

        if (previous != null && record.DateAdministered.Date < previous.DateAdministered.Date)
        {
            throw new InvalidOperationException(
                "Dose " + dose.DoseNumber + " cannot be dated before the previous dose (" +
                previous.DateAdministered.ToString("yyyy-MM-dd") + ").");
        }

        try
        {
            return _dataAccess.Add(record);
        }
        catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)   // duplicate key
        {
            throw new InvalidOperationException("This dose has already been recorded for this child.");
        }
    }

    public int GetTotalVaccinationCount()
    {
        return _dataAccess.CountRecords();
    }
}
