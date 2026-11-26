// Folder: Services/ChildService.cs
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ImmunoTrack.DataAccess;
using ImmunoTrack.Models;

namespace ImmunoTrack.Services
{
    /// <summary>The three ways a health worker can search for a child.</summary>
    public enum ChildSearchType
    {
        ChildID = 0,
        ChildName = 1,
        GuardianName = 2
    }

    /// <summary>
    /// Business layer for children. Applies the application rules:
    /// no duplicate registrations, friendly messages for database conflicts,
    /// and routing each search type to the right query.
    /// </summary>
    public class ChildService
    {
        private readonly ChildDataAccess _dataAccess = new ChildDataAccess();

        public List<Child> GetAllChildren()
        {
            return _dataAccess.GetAll();
        }

        public Child? GetChild(int childId)
        {
            return _dataAccess.GetById(childId);
        }

        // Blank search text returns everybody.
        public List<Child> SearchChildren(ChildSearchType searchType, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return _dataAccess.GetAll();

            string text = searchText.Trim();

            switch (searchType)
            {
                case ChildSearchType.ChildID:
                {
                    if (!int.TryParse(text, out int childId))
                        throw new ArgumentException("Child ID must be a number.");

                    Child? found = _dataAccess.GetById(childId);
                    return found == null ? new List<Child>() : new List<Child> { found };
                }

                case ChildSearchType.GuardianName:
                    return _dataAccess.SearchByGuardianName(text);

                default:
                    return _dataAccess.SearchByChildName(text);
            }
        }

        public int AddChild(Child child)
        {
            // Business rule: the same child must not be registered twice.
            if (_dataAccess.ExistsDuplicate(child.FirstName, child.LastName, child.DateOfBirth, 0))
                throw new InvalidOperationException(
                    "A child with the same name and date of birth is already registered.");

            return _dataAccess.Add(child);
        }

        public void UpdateChild(Child child)
        {
            if (child.ChildID <= 0)
                throw new InvalidOperationException("Select a child to update first.");

            if (_dataAccess.ExistsDuplicate(child.FirstName, child.LastName, child.DateOfBirth, child.ChildID))
                throw new InvalidOperationException(
                    "Another child with the same name and date of birth is already registered.");

            _dataAccess.Update(child);
        }

        public void DeleteChild(int childId)
        {
            if (childId <= 0)
                throw new InvalidOperationException("Select a child to delete first.");

            try
            {
                _dataAccess.Delete(childId);
            }
            catch (SqlException ex) when (ex.Number == 547)   // foreign key conflict
            {
                throw new InvalidOperationException(
                    "This child already has vaccination records, so they cannot be deleted. " +
                    "Immunization history must be kept.");
            }
        }
    }
}
