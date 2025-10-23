using System.Security.Cryptography;

namespace billing_system
{
    internal static class PasswordHasher
    {
        public static (byte[] hash, byte[] salt) HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(AppConfig.SaltSize);
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password, salt, AppConfig.Pbkdf2Iter, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(AppConfig.HashSize);
            return (hash, salt);
        }

        public static bool Verify(string password, byte[] hash, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password, salt, AppConfig.Pbkdf2Iter, HashAlgorithmName.SHA256);
            byte[] computed = pbkdf2.GetBytes(AppConfig.HashSize);
            return CryptographicOperations.FixedTimeEquals(computed, hash);
        }
    }
}
