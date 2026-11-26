// Folder: Models/Person.cs
using System;
using ImmunoTrack.Helpers;

namespace ImmunoTrack.Models
{
    /// <summary>
    /// Base class for every person in the system (Child, Guardian, HealthWorker).
    /// Demonstrates: ABSTRACTION (abstract class), ENCAPSULATION (private fields +
    /// validated properties), POLYMORPHISM (abstract/virtual methods).
    /// </summary>
    public abstract class Person
    {
        // ENCAPSULATION: private fields, only reachable through the properties below.
        private string _firstName = string.Empty;
        private string? _middleName;
        private string _lastName = string.Empty;

        // CONSTRUCTORS
        protected Person()
        {
        }

        protected Person(int personId, string firstName, string? middleName, string lastName)
        {
            PersonID = personId;
            FirstName = firstName;     // goes through the validating setter
            MiddleName = middleName;
            LastName = lastName;
        }

        public int PersonID { get; set; }

        public string FirstName
        {
            get => _firstName;
            set => _firstName = InputValidator.RequireText(value, "First name", 50);
        }

        public string? MiddleName
        {
            get => _middleName;
            set => _middleName = InputValidator.OptionalText(value, "Middle name", 50);
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = InputValidator.RequireText(value, "Last name", 50);
        }

        // Computed property: nothing stored, always built from the other fields.
        public string FullName =>
            string.IsNullOrEmpty(MiddleName)
                ? $"{FirstName} {LastName}"
                : $"{FirstName} {MiddleName} {LastName}";

        // POLYMORPHISM: every derived class MUST provide its own version.
        public abstract string GetDisplayInfo();

        // POLYMORPHISM: derived classes MAY override this one.
        public virtual string GetRoleName()
        {
            return "Person";
        }
    }
}
