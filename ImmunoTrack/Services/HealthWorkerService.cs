// Folder: Services/HealthWorkerService.cs
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ImmunoTrack.DataAccess;
using ImmunoTrack.Helpers;
using ImmunoTrack.Models;

namespace ImmunoTrack.Services
{
    /// <summary>
    /// Business layer for health workers: unique usernames, password rules and
    /// hashing, and protection against deleting the last account (which would
    /// lock everyone out of the system).
    /// </summary>
    public class HealthWorkerService
    {
        private const int MinPasswordLength = 6;

        private readonly HealthWorkerDataAccess _dataAccess = new HealthWorkerDataAccess();

        public List<HealthWorker> GetAllHealthWorkers()
        {
            return _dataAccess.GetAll();
        }

        public HealthWorker? GetHealthWorker(int healthWorkerId)
        {
            return _dataAccess.GetById(healthWorkerId);
        }

        // Blank search text returns everybody.
        public List<HealthWorker> SearchHealthWorkers(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return _dataAccess.GetAll();

            return _dataAccess.Search(searchText.Trim());
        }

        public int AddHealthWorker(HealthWorker worker, string password)
        {
            ValidatePassword(password);

            if (_dataAccess.UsernameExists(worker.Username, 0))
                throw new InvalidOperationException("That username is already taken. Choose another one.");

            string salt = PasswordHasher.GenerateSalt();
            byte[] hash = PasswordHasher.Hash(salt, password);
            return _dataAccess.Add(worker, salt, hash);
        }

        public void UpdateHealthWorker(HealthWorker worker)
        {
            if (worker.HealthWorkerID <= 0)
                throw new InvalidOperationException("Select a health worker to update first.");

            if (_dataAccess.UsernameExists(worker.Username, worker.HealthWorkerID))
                throw new InvalidOperationException("That username is already taken. Choose another one.");

            // The screen has no Role box, so keep the role the account already has.
            // (This is what stops an Admin from being turned into Staff.)
            HealthWorker? existing = _dataAccess.GetById(worker.HealthWorkerID);
            if (existing == null)
                throw new InvalidOperationException("That health worker no longer exists.");
            worker.Role = existing.Role;

            _dataAccess.Update(worker);
        }

        public void ResetPassword(int healthWorkerId, string newPassword)
        {
            if (healthWorkerId <= 0)
                throw new InvalidOperationException("Select a health worker first.");

            ValidatePassword(newPassword);

            string salt = PasswordHasher.GenerateSalt();
            byte[] hash = PasswordHasher.Hash(salt, newPassword);
            _dataAccess.UpdatePassword(healthWorkerId, salt, hash);
        }

        public void DeleteHealthWorker(int healthWorkerId)
        {
            if (healthWorkerId <= 0)
                throw new InvalidOperationException("Select a health worker to delete first.");

            if (UserSession.CurrentUser != null && UserSession.CurrentUser.HealthWorkerID == healthWorkerId)
                throw new InvalidOperationException("You cannot delete the account you are currently logged in with.");

            HealthWorker? target = _dataAccess.GetById(healthWorkerId);
            if (target != null && target.IsAdmin)
                throw new InvalidOperationException("Administrator accounts cannot be deleted.");

            // Business rule: never delete the last account, or nobody could log in.
            if (_dataAccess.GetAll().Count <= 1)
                throw new InvalidOperationException(
                    "You cannot delete the only health worker account. The system needs at least one.");

            try
            {
                _dataAccess.Delete(healthWorkerId);
            }
            catch (SqlException ex) when (ex.Number == 547)   // foreign key conflict
            {
                throw new InvalidOperationException(
                    "This health worker has recorded vaccinations, so they cannot be deleted. " +
                    "Vaccination history must be kept.");
            }
        }

        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < MinPasswordLength)
                throw new ArgumentException($"Password must be at least {MinPasswordLength} characters.");
        }
    }
}