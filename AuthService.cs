namespace billing_system
{
    internal sealed class AuthResult
    {
        public bool IsAuthenticated { get; init; }
        public string? Error { get; init; }
        public User? User { get; init; }
    }

    internal sealed class AuthService
    {
        private readonly UserRepository _users = new UserRepository();
        public User? CurrentUser { get; private set; }

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

        public void Logout()
        {
            AppSession.CurrentUser = null;
            CurrentUser = null;
        }
    }
}
