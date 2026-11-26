// Folder: Services/AuthService.cs
using System;
using ImmunoTrack.DataAccess;
using ImmunoTrack.Helpers;
using ImmunoTrack.Models;

namespace ImmunoTrack.Services
{
    /// <summary>
    /// Checks a username and password. The password is never compared as text:
    /// we hash what the user typed (with the stored salt) and compare the hashes.
    /// </summary>
    public class AuthService
    {
        private readonly HealthWorkerDataAccess _dataAccess = new HealthWorkerDataAccess();

        // Returns the logged-in health worker, or throws if the login is wrong.
        public HealthWorker Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Please enter your username and password.");

            StoredCredentials? credentials = _dataAccess.GetCredentials(username.Trim());

            // Same message for "no such user" and "wrong password", so a stranger
            // cannot find out which usernames exist.
            if (credentials == null ||
                !PasswordHasher.Verify(credentials.PasswordSalt, password, credentials.PasswordHash))
            {
                throw new InvalidOperationException("Incorrect username or password.");
            }

            HealthWorker? worker = _dataAccess.GetById(credentials.HealthWorkerID);
            if (worker == null)
                throw new InvalidOperationException("Incorrect username or password.");

            return worker;
        }
    }
}
