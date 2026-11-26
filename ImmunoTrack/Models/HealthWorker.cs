// Folder: Models/HealthWorker.cs
using System;
using ImmunoTrack.Helpers;

namespace ImmunoTrack.Models
{
    /// <summary>
    /// A barangay health worker who uses the system and records vaccinations.
    /// INHERITS from Person. The password hash/salt are NOT kept in this class:
    /// the login code (added later) reads and compares them separately.
    /// </summary>
    public class HealthWorker : Person
    {
        private string _contactNumber = string.Empty;
        private string _position = string.Empty;
        private string? _licenseNo;
        private string _username = string.Empty;
        private string _role = StaffRole;

        public const string AdminRole = "Admin";
        public const string StaffRole = "Staff";

        public HealthWorker()
        {
        }

        public HealthWorker(int healthWorkerId, string firstName, string? middleName, string lastName,
                            string contactNumber, string position, string? licenseNo, string username,
                            string role = StaffRole)
            : base(healthWorkerId, firstName, middleName, lastName)
        {
            ContactNumber = contactNumber;
            Position = position;
            LicenseNo = licenseNo;
            Username = username;
            Role = role;
        }

        public int HealthWorkerID
        {
            get => PersonID;
            set => PersonID = value;
        }

        public string ContactNumber
        {
            get => _contactNumber;
            set => _contactNumber = InputValidator.RequireContactNumber(value);
        }

        public string Position
        {
            get => _position;
            set => _position = InputValidator.RequireText(value, "Position", 50);
        }

        public string? LicenseNo
        {
            get => _licenseNo;
            set => _licenseNo = InputValidator.OptionalText(value, "License number", 30);
        }

        public string Username
        {
            get => _username;
            set
            {
                string cleaned = InputValidator.RequireText(value, "Username", 30);
                if (cleaned.Length < 3)
                    throw new ArgumentException("Username must be at least 3 characters.");
                _username = cleaned;
            }
        }

        // "Admin" or "Staff". Admins cannot be deleted or demoted (see HealthWorkerService).
        public string Role
        {
            get => _role;
            set
            {
                if (value != AdminRole && value != StaffRole)
                    throw new ArgumentException("Role must be Admin or Staff.");
                _role = value;
            }
        }

        public bool IsAdmin => _role == AdminRole;

        public override string GetDisplayInfo()
        {
            return $"{FullName} | {Position}";
        }

        public override string GetRoleName()
        {
            return IsAdmin ? "Administrator" : "Health Worker";
        }
    }
}