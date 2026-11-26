// Folder: Models/Child.cs
using System;
using System.Linq;
using ImmunoTrack.Helpers;

namespace ImmunoTrack.Models
{
    /// <summary>
    /// A child registered in the barangay. INHERITS from Person.
    /// </summary>
    public class Child : Person
    {
        private static readonly string[] ValidBloodTypes =
            { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };

        private DateTime _dateOfBirth;
        private string _sex = "Male";
        private string? _bloodType;
        private string _address = string.Empty;

        public Child()
        {
        }

        public Child(int childId, string firstName, string? middleName, string lastName,
                     DateTime dateOfBirth, string sex, string? bloodType,
                     string address, int guardianId)
            : base(childId, firstName, middleName, lastName)   // call the Person constructor
        {
            DateOfBirth = dateOfBirth;
            Sex = sex;
            BloodType = bloodType;
            Address = address;
            GuardianID = guardianId;
        }

        // Same value as PersonID, named to match the database column.
        public int ChildID
        {
            get => PersonID;
            set => PersonID = value;
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (value.Date > DateTime.Today)
                    throw new ArgumentException("Date of birth cannot be in the future.");
                _dateOfBirth = value.Date;
            }
        }

        public string Sex
        {
            get => _sex;
            set
            {
                if (value != "Male" && value != "Female")
                    throw new ArgumentException("Sex must be Male or Female.");
                _sex = value;
            }
        }

        public string? BloodType
        {
            get => _bloodType;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _bloodType = null;      // blood type is optional
                    return;
                }

                string cleaned = value.Trim().ToUpper();
                if (!ValidBloodTypes.Contains(cleaned))
                    throw new ArgumentException("Blood type must be A+, A-, B+, B-, AB+, AB-, O+ or O-.");
                _bloodType = cleaned;
            }
        }

        public string Address
        {
            get => _address;
            set => _address = InputValidator.RequireText(value, "Address", 200);
        }

        public int GuardianID { get; set; }

        // Used by the vaccination schedule calculation (ages are stored in days).
        public int GetAgeInDays()
        {
            return (DateTime.Today - DateOfBirth).Days;
        }

        public string GetAgeText()
        {
            int days = GetAgeInDays();
            if (days < 30)
                return days == 1 ? "1 day" : $"{days} days";

            int months = days / 30;
            if (months < 24)
                return months == 1 ? "1 month" : $"{months} months";

            return $"{months / 12} years";
        }

        // POLYMORPHISM: Child's own version of the abstract method.
        public override string GetDisplayInfo()
        {
            return $"{FullName} | {Sex} | {GetAgeText()} old";
        }

        public override string GetRoleName()
        {
            return "Child";
        }
    }
}
