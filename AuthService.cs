namespace billing_system
{
    /// <summary>
    /// Represents the result of an authentication attempt.
    /// </summary>
    /// <remarks>
    /// This class encapsulates the outcome of a login operation, indicating whether the authentication was successful
    /// and providing the authenticated user object or an error message. It is returned by the <see cref="AuthService.Login"/> method.
    /// </remarks>
    internal sealed class AuthResult
    {
        /// <summary>
        /// Gets a value indicating whether the authentication was successful.
        /// </summary>
        public bool IsAuthenticated { get; init; }

        /// <summary>
        /// Gets the error message if authentication failed.
        /// </summary>
        public string? Error { get; init; }

        /// <summary>
        /// Gets the authenticated <see cref="User"/> object if authentication was successful.
        /// </summary>
        public User? User { get; init; }
    }

    /// <summary>
    /// Handles user authentication logic.
    /// </summary>
    /// <remarks>
    /// This service class is responsible for authenticating users by verifying their credentials.
    /// It collaborates with the <see cref="UserRepository"/> to retrieve user data and the <see cref="PasswordHasher"/>
    /// to validate passwords. It also manages the user's session state via <see cref="AppSession"/>.
    /// </remarks>
    internal sealed class AuthService
    {
        /// <summary>
        /// The repository for accessing user data.
        /// </summary>
        private readonly UserRepository _users = new UserRepository();

        /// <summary>
        /// Gets the currently authenticated user for the lifetime of this service instance.
        /// </summary>
        /// <remarks>
        /// This property is set upon a successful login. While <see cref="AppSession.CurrentUser"/> provides a global
        /// session state, this property can be used for instance-specific tracking if needed.
        /// </remarks>
        public User? CurrentUser { get; private set; }

        /// <summary>
        /// Attempts to authenticate a user with the provided credentials.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>An <see cref="AuthResult"/> object containing the outcome of the authentication attempt.</returns>
        /// <remarks>
        /// This method performs the following steps:
        /// 1. Validates that the username and password are not empty.
        /// 2. Retrieves the user, password hash, and salt from the <see cref="UserRepository"/>.
        /// 3. Checks if the user exists and is active.
        /// 4. Verifies the provided password against the stored hash using <see cref="PasswordHasher.Verify"/>.
        /// 5. If all checks pass, it sets the <see cref="AppSession.CurrentUser"/> to establish a global session
        ///    and returns a successful <see cref="AuthResult"/>.
        /// 6. If any check fails, it returns a failed <see cref="AuthResult"/> with an appropriate error message.
        /// </remarks>
        public AuthResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return new AuthResult { IsAuthenticated = false, Error = "Please enter username and password." };

            var (user, hash, salt) = _users.GetByUsername(username);
            if (user is null || hash is null || salt is null)
                return new AuthResult { IsAuthenticated = false, Error = "Invalid username or password." };

            if (!user.IsActive)
                return new AuthResult { IsAuthenticated = false, Error = "Account is disabled." };

            if (!PasswordHasher.Verify(password, hash, salt))
                return new AuthResult { IsAuthenticated = false, Error = "Invalid username or password." };

            AppSession.CurrentUser = user;
            CurrentUser = user;

            return new AuthResult { IsAuthenticated = true, User = user };
        }

        /// <summary>
        /// Logs out the current user by clearing the session state.
        /// </summary>
        /// <remarks>
        /// This method sets both the global <see cref="AppSession.CurrentUser"/> and the local <see cref="CurrentUser"/>
        /// properties to <c>null</c>, effectively ending the user's session.
        /// </remarks>
        public void Logout()
        {
            AppSession.CurrentUser = null;
            CurrentUser = null;
        }
    }
}
