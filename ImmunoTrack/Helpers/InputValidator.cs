// Folder: Helpers/InputValidator.cs
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ImmunoTrack.Helpers
{
    /// <summary>
    /// Reusable validation rules. The model classes call these inside their
    /// property setters, so invalid data can never be stored in an object.
    /// </summary>
    public static class InputValidator
    {
        // Required text: not empty, trimmed, not longer than maxLength.
        public static string RequireText(string? value, string fieldName, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{fieldName} is required.");

            string trimmed = value.Trim();
            if (trimmed.Length > maxLength)
                throw new ArgumentException($"{fieldName} must not exceed {maxLength} characters.");

            return trimmed;
        }

        // Optional text: returns null when blank (so the database stores NULL).
        public static string? OptionalText(string? value, string fieldName, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            string trimmed = value.Trim();
            if (trimmed.Length > maxLength)
                throw new ArgumentException($"{fieldName} must not exceed {maxLength} characters.");

            return trimmed;
        }

        // Contact number: digits, +, space and - only, with at least 7 digits.
        // (Matches the CHECK constraint in the database.)
        public static string RequireContactNumber(string? value)
        {
            string number = RequireText(value, "Contact number", 20);

            if (!Regex.IsMatch(number, @"^[0-9+\- ]+$"))
                throw new ArgumentException("Contact number may only contain digits, +, - and spaces.");

            if (number.Count(char.IsDigit) < 7)
                throw new ArgumentException("Contact number must have at least 7 digits.");

            return number;
        }
    }
}
