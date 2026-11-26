// Folder: Helpers/PasswordHasher.cs
using System;
using System.Security.Cryptography;
using System.Text;

namespace ImmunoTrack.Helpers
{
    /// <summary>
    /// Passwords are NEVER stored as plain text. Each health worker gets a random
    /// "salt" and we store SHA-256(salt + password). To check a login we repeat the
    /// same calculation and compare. The formula matches the sample data in
    /// InsertSampleData.sql, so the sample accounts can log in.
    /// </summary>
    public static class PasswordHasher
    {
        // 16 random bytes written as 32 hex characters (fits VARCHAR(32)).
        public static string GenerateSalt()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
        }

        public static byte[] Hash(string salt, string password)
        {
            return SHA256.HashData(Encoding.UTF8.GetBytes(salt + password));
        }

        public static bool Verify(string salt, string password, byte[] storedHash)
        {
            byte[] attempt = Hash(salt, password);
            return CryptographicOperations.FixedTimeEquals(attempt, storedHash);
        }
    }
}
