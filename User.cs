namespace billing_system
{
    /// <summary>
    /// Represents a user account in the system.
    /// </summary>
    /// <remarks>
    /// This class is a data model that holds information about a user, such as their credentials, role, and status.
    /// It is populated with data retrieved from the 'Users' table by the <see cref="UserRepository"/> and used by the
    /// <see cref="AuthService"/> during the authentication process.
    /// </remarks>
    internal sealed class User
    {
        /// <summary>
        /// Gets the unique identifier for the user.
        /// </summary>
        public int UserId { get; init; }

        /// <summary>
        /// Gets the user's login name.
        /// </summary>
        public string Username { get; init; } = "";

        /// <summary>
        /// Gets the user's full name.
        /// </summary>
        public string FullName { get; init; } = "";

        /// <summary>
        /// Gets the user's role, which determines their permissions.
        /// </summary>
        /// <remarks>
        /// Example roles include "Admin" or "Cashier".
        /// </remarks>
        public string Role { get; init; } = "Cashier";

        /// <summary>
        // Gets a value indicating whether the user's account is active.
        /// </summary>
        /// <remarks>
        /// Inactive users are not able to log in.
        /// </remarks>
        public bool IsActive { get; init; } = true;

        /// <summary>
        /// Gets or sets the timestamp of the user's last login.
        /// </summary>
        public string? LastLogin { get; set; }
    }
}
