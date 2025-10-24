namespace billing_system
{
    /// <summary>
    /// Manages the application's session state.
    /// </summary>
    /// <remarks>
    /// This static class holds session-specific data, such as the currently logged-in user.
    /// It provides a central point of access for user information throughout the application's lifecycle.
    /// </remarks>
    internal static class AppSession
    {
        /// <summary>
        /// Gets or sets the currently authenticated user.
        /// </summary>
        /// <remarks>
        /// This property holds the <see cref="User"/> object for the user who is currently logged in.
        /// It is set by the <see cref="AuthService.Login"/> method upon successful authentication and cleared upon logout.
        /// Various parts of the application, such as the <see cref="MainShellForm"/> and <see cref="CashierPOSForm"/>, access this property to determine the user's identity and role.
        /// A <c>null</c> value indicates that no user is currently logged in.
        /// </remarks>
        public static User? CurrentUser { get; set; }
    }
}
