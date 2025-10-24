using System;
using System.Data;
using System.Data.SQLite;

namespace billing_system
{
    /// <summary>
    /// Manages data access for user-related operations.
    /// </summary>
    /// <remarks>
    /// This class provides a set of methods for creating, reading, updating, and deleting user records in the database.
    /// It interacts directly with the <see cref="Database"/> class to execute SQL queries and uses the <see cref="PasswordHasher"/>
    /// for handling password security. It is primarily used by the <see cref="AuthService"/> and <see cref="UserManagementControl"/>.
    /// </remarks>
    internal sealed class UserRepository
    {
        /// <summary>
        /// Retrieves a user's details, including their password hash and salt, by their username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>A tuple containing the <see cref="User"/> object, the password hash, and the password salt.
        /// Returns (<c>null</c>, <c>null</c>, <c>null</c>) if the user is not found.</returns>
        /// <remarks>
        /// This method is primarily used by the <see cref="AuthService"/> during the login process to get the necessary data
        /// for password verification.
        /// </remarks>
        public (User? user, byte[]? hash, byte[]? salt) GetByUsername(string username)
        {
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT user_id, username, full_name, role, is_active,
                       password_hash, password_salt
                FROM Users
                WHERE username = @u;";
            cmd.Parameters.AddWithValue("@u", username);

            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return (null, null, null);

            var user = new User
            {
                UserId = rd.GetInt32(0),
                Username = rd.GetString(1),
                FullName = rd.GetString(2),
                Role = rd.GetString(3),
                IsActive = rd.GetInt32(4) == 1
            };

            byte[] hash = (byte[])rd["password_hash"];
            byte[] salt = (byte[])rd["password_salt"];
            return (user, hash, salt);
        }

        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="username">The username for the new user.</param>
        /// <param name="password">The plain-text password for the new user.</param>
        /// <param name="fullName">The full name of the new user.</param>
        /// <param name="role">The role of the new user. Defaults to "Cashier".</param>
        /// <param name="isActive">A value indicating whether the new user is active. Defaults to <c>true</c>.</param>
        /// <remarks>
        /// This method first hashes the provided password using <see cref="PasswordHasher.HashPassword"/> and then
        /// inserts the new user's details into the 'Users' table. This is used by the <see cref="UserDetailForm"/> to add new users.
        /// </remarks>
        public void CreateUser(string username, string password, string fullName, string role = "Cashier", bool isActive = true)
        {
            var (hash, salt) = PasswordHasher.HashPassword(password);
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Users (username, password_hash, password_salt, full_name, role, is_active)
                VALUES (@u, @h, @s, @n, @r, @a);";
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@h", hash);
            cmd.Parameters.AddWithValue("@s", salt);
            cmd.Parameters.AddWithValue("@n", fullName);
            cmd.Parameters.AddWithValue("@r", role);
            cmd.Parameters.AddWithValue("@a", isActive ? 1 : 0);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing all users' details.</returns>
        /// <remarks>
        /// This method is designed to fetch a list of all users for display in a data grid, such as in the <see cref="UserManagementControl"/>.
        /// The password hash and salt are excluded for security reasons.
        /// </remarks>
        public DataTable GetAllUsers()
        {
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT user_id, username, full_name, role, is_active, created_at
                FROM Users
                ORDER BY user_id;";
            using var da = new SQLiteDataAdapter(cmd);
            var table = new DataTable();
            da.Fill(table);
            return table;
        }

        /// <summary>
        /// Checks if a username already exists in the database.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <param name="excludeUserId">An optional user ID to exclude from the check.
        /// This is used when updating an existing user to ensure the new username is not taken by someone else.</param>
        /// <returns><c>true</c> if the username exists; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This provides a clean way to check for username uniqueness before attempting an insert or update,
        /// which is useful in forms like <see cref="UserDetailForm"/>.
        /// </remarks>
        public bool UsernameExists(string username, int? excludeUserId = null)
        {
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();

            if (excludeUserId.HasValue)
            {
                cmd.CommandText = "SELECT 1 FROM Users WHERE username = @u AND user_id <> @id LIMIT 1;";
                cmd.Parameters.AddWithValue("@id", excludeUserId.Value);
            }
            else
            {
                cmd.CommandText = "SELECT 1 FROM Users WHERE username = @u LIMIT 1;";
            }

            cmd.Parameters.AddWithValue("@u", username);
            using var rd = cmd.ExecuteReader();
            return rd.Read();
        }

        /// <summary>
        /// Updates a user's details in the database.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="username">The user's new username.</param>
        /// <param name="fullName">The user's new full name.</param>
        /// <param name="role">The user's new role.</param>
        /// <param name="isActive">The user's new active status.</param>
        /// <remarks>
        /// This method first checks if the new username is already taken by another user before applying the update.
        /// It is used by the <see cref="UserDetailForm"/> to save changes to a user's profile.
        /// This method does not update the password. For that, use <see cref="UpdatePassword"/>.
        /// </remarks>
        public void UpdateUser(int userId, string username, string fullName, string role, bool isActive)
        {
            using var conn = Database.GetConnection();
            conn.Open();

            if (UsernameExists(username, userId))
                throw new InvalidOperationException("Username already exists.");

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Users
                SET username = @u,
                    full_name = @n,
                    role      = @r,
                    is_active = @a
                WHERE user_id = @id;";
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@n", fullName);
            cmd.Parameters.AddWithValue("@r", role);
            cmd.Parameters.AddWithValue("@a", isActive ? 1 : 0);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Toggles a user's active status.
        /// </summary>
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="isActive">The new active status.</param>
        /// <remarks>
        /// This is a convenience method for quickly activating or deactivating a user.
        /// </remarks>
        public void ToggleActive(int userId, bool isActive)
        {
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Users SET is_active = @a WHERE user_id = @id;";
            cmd.Parameters.AddWithValue("@a", isActive ? 1 : 0);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates a user's password.
        /// </summary>
        /// <param name="userId">The ID of the user whose password is to be updated.</param>
        /// <param name="newPassword">The user's new plain-text password.</param>
        /// <remarks>
        /// This method hashes the new password using <see cref="PasswordHasher.HashPassword"/> and updates the
        /// corresponding hash and salt in the database. It is used by the <see cref="UserDetailForm"/> to reset a user's password.
        /// </remarks>
        public void UpdatePassword(int userId, string newPassword)
        {
            var (hash, salt) = PasswordHasher.HashPassword(newPassword);
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Users
                SET password_hash = @h,
                    password_salt = @s
                WHERE user_id = @id;";
            cmd.Parameters.AddWithValue("@h", hash);
            cmd.Parameters.AddWithValue("@s", salt);
            cmd.Parameters.AddWithValue("@id", userId);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Retrieves a single user's details by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>A tuple containing the user's details, or <c>null</c> if the user is not found.</returns>
        /// <remarks>
        /// This is a helper method used to fetch a user's data for editing in forms like <see cref="UserDetailForm"/>.
        /// </remarks>
        public (int UserId, string Username, string FullName, string Role, bool IsActive, DateTime CreatedAt)? GetById(int userId)
        {
            using var conn = Database.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT user_id, username, full_name, role, is_active, created_at
                FROM Users
                WHERE user_id = @id;";
            cmd.Parameters.AddWithValue("@id", userId);
            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;

            var createdAtText = rd["created_at"]?.ToString() ?? "";
            DateTime.TryParse(createdAtText, out var createdAt);

            return (rd.GetInt32(0), rd.GetString(1), rd.GetString(2), rd.GetString(3),
                    rd.GetInt32(4) == 1, createdAt);
        }
    }
}
