using System.Security.Cryptography;

namespace billing_system
{
    /// <summary>
    /// Provides methods for hashing and verifying passwords using a secure algorithm.
    /// </summary>
    /// <remarks>
    /// This static class uses the PBKDF2 algorithm (Rfc2898DeriveBytes) with a SHA256 hash function to ensure that passwords are stored securely.
    /// It relies on configuration values from <see cref="AppConfig"/> for salt size, hash size, and iteration count.
    /// </remarks>
    internal static class PasswordHasher
    {
        /// <summary>
        /// Hashes a password using a randomly generated salt.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A tuple containing the generated hash and salt as byte arrays.</returns>
        /// <remarks>
        /// This method generates a new salt of the size specified in <see cref="AppConfig.SaltSize"/>.
        /// It then uses the PBKDF2 algorithm to derive a hash of the size specified in <see cref="AppConfig.HashSize"/>.
        /// The iteration count is taken from <see cref="AppConfig.Pbkdf2Iter"/>.
        /// This method is called by <see cref="UserRepository"/> when creating a new user or updating a password.
        /// </remarks>
        public static (byte[] hash, byte[] salt) HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(AppConfig.SaltSize);
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password, salt, AppConfig.Pbkdf2Iter, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(AppConfig.HashSize);
            return (hash, salt);
        }

        /// <summary>
        /// Verifies a password against a stored hash and salt.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="hash">The stored password hash.</param>
        /// <param name="salt">The stored salt.</param>
        /// <returns><c>true</c> if the password is correct; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method re-computes the hash of the provided password using the stored salt and the same PBKDF2 parameters.
        /// It then performs a fixed-time comparison of the computed hash and the stored hash to prevent timing attacks.
        /// This method is used by the <see cref="AuthService"/> to authenticate users during login.
        /// </remarks>
        public static bool Verify(string password, byte[] hash, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password, salt, AppConfig.Pbkdf2Iter, HashAlgorithmName.SHA256);
            byte[] computed = pbkdf2.GetBytes(AppConfig.HashSize);
            return CryptographicOperations.FixedTimeEquals(computed, hash);
        }
    }
}
