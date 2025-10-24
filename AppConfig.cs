namespace billing_system
{
    /// <summary>
    /// Provides application-wide configuration constants.
    /// </summary>
    /// <remarks>
    /// This static class holds constants for database connection, password hashing parameters, and other global settings.
    /// </remarks>
    internal static class AppConfig
    {
        /// <summary>
        /// Gets the path to the SQLite database file.
        /// </summary>
        /// <remarks>
        /// This constant specifies the name of the database file. The application will create this file in its working directory if it doesn't exist.
        /// It is used by the <see cref="Database.GetConnection"/> method to establish a connection to the database.
        /// </remarks>
        public const string DbPath = "bims.db";

        /// <summary>
        /// Gets the number of iterations for the PBKDF2 hashing algorithm.
        /// </summary>
        /// <remarks>
        /// This constant defines the computational cost of hashing a password. A higher number increases security but also slows down the hashing process.
        /// This value is used by the <see cref="PasswordHasher"/> class.
        /// </remarks>
        public const int Pbkdf2Iter = 100_000;

        /// <summary>
        /// Gets the size of the salt in bytes for password hashing.
        /// </summary>
        /// <remarks>
        /// A salt is a random value added to a password before hashing to prevent rainbow table attacks. This constant defines the length of that salt.
        /// This value is used by the <see cref="PasswordHasher"/> class.
        /// </remarks>
        public const int SaltSize = 16;

        /// <summary>
        /// Gets the size of the hash in bytes for password hashing.
        /// </summary>
        /// <remarks>
        /// This constant defines the desired length of the generated password hash.
        /// This value is used by the <see cref="PasswordHasher"/> class.
        /// </remarks>
        public const int HashSize = 32;
    }
}
