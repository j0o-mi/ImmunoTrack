// Folder: Models/Guardian.cs
using System;
using ImmunoTrack.Helpers;

namespace ImmunoTrack.Models
{
    /// <summary>
    /// A parent or guardian responsible for one or more children. INHERITS from Person.
    /// </summary>
    public class Guardian : Person
    {
        private string _contactNumber = string.Empty;
        private string _address = string.Empty;
        private string _relationshipToChild = string.Empty;

        public Guardian()
        {
        }

        public Guardian(int guardianId, string firstName, string? middleName, string lastName,
                        string contactNumber, string address, string relationshipToChild)
            : base(guardianId, firstName, middleName, lastName)
        {
            ContactNumber = contactNumber;
            Address = address;
            RelationshipToChild = relationshipToChild;
        }

        public int GuardianID
        {
            get => PersonID;
            set => PersonID = value;
        }

        public string ContactNumber
        {
            get => _contactNumber;
            set => _contactNumber = InputValidator.RequireContactNumber(value);
        }

        public string Address
        {
            get => _address;
            set => _address = InputValidator.RequireText(value, "Address", 200);
        }

        public string RelationshipToChild
        {
            get => _relationshipToChild;
            set => _relationshipToChild = InputValidator.RequireText(value, "Relationship to child", 30);
        }

        public override string GetDisplayInfo()
        {
            return $"{FullName} ({RelationshipToChild}) | {ContactNumber}";
        }

        public override string GetRoleName()
        {
            return "Guardian";
        }
    }
}
