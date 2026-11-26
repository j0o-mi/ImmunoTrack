// Folder: Services/GuardianService.cs
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ImmunoTrack.DataAccess;
using ImmunoTrack.Models;

namespace ImmunoTrack.Services
{
    /// <summary>
    /// Business layer for guardians. Forms talk to THIS class, never to the
    /// database directly. It applies application rules and turns technical
    /// database errors into messages a health worker can understand.
    /// (Field validation already happens inside the Guardian class itself.)
    /// </summary>
    public class GuardianService
    {
        private readonly GuardianDataAccess _dataAccess = new GuardianDataAccess();

        public List<Guardian> GetAllGuardians()
        {
            return _dataAccess.GetAll();
        }

        public Guardian? GetGuardian(int guardianId)
        {
            return _dataAccess.GetById(guardianId);
        }

        // Blank search text shows everybody.
        public List<Guardian> SearchGuardians(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return _dataAccess.GetAll();

            return _dataAccess.Search(searchText.Trim());
        }

        public int AddGuardian(Guardian guardian)
        {
            return _dataAccess.Add(guardian);
        }

        public void UpdateGuardian(Guardian guardian)
        {
            if (guardian.GuardianID <= 0)
                throw new InvalidOperationException("Select a guardian to update first.");

            _dataAccess.Update(guardian);
        }

        public void DeleteGuardian(int guardianId)
        {
            if (guardianId <= 0)
                throw new InvalidOperationException("Select a guardian to delete first.");

            try
            {
                _dataAccess.Delete(guardianId);
            }
            catch (SqlException ex) when (ex.Number == 547)   // foreign key conflict
            {
                throw new InvalidOperationException(
                    "This guardian still has registered children, so they cannot be deleted. " +
                    "Remove or reassign the children first.");
            }
        }
    }
}
