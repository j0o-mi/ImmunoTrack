// Folder: Models/Vaccine.cs
using System;
using ImmunoTrack.Helpers;

namespace ImmunoTrack.Models
{
    /// <summary>
    /// A vaccine in the reference list (name only). This is NOT inventory:
    /// no stock, batches or expiry dates are tracked anywhere in the system.
    /// </summary>
    public class Vaccine
    {
        private string _vaccineName = string.Empty;
        private string? _description;

        public Vaccine()
        {
        }

        public Vaccine(int vaccineId, string vaccineName, string? description, bool isActive)
        {
            VaccineID = vaccineId;
            VaccineName = vaccineName;
            Description = description;
            IsActive = isActive;
        }

        public int VaccineID { get; set; }

        public string VaccineName
        {
            get => _vaccineName;
            set => _vaccineName = InputValidator.RequireText(value, "Vaccine name", 100);
        }

        public string? Description
        {
            get => _description;
            set => _description = InputValidator.OptionalText(value, "Description", 250);
        }

        // Inactive vaccines are ignored when calculating due doses and defaulters.
        public bool IsActive { get; set; } = true;

        public override string ToString()
        {
            return VaccineName;
        }
    }
}
