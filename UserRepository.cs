using System;
using System.Data;
using System.Data.SQLite;

namespace billing_system
{
    internal sealed class UserRepository
    {
        // Existing methods you already had:
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

        // NEW: Fetch everything for grid binding.
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

        // NEW: Friendly uniqueness check (lets us show a clean message instead of raw SQLite error)
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

        // NEW: Update basic fields for Edit mode
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

        // NEW: Toggle active (unused directly because we do it via Edit dialog, but handy)
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

        // NEW: Reset password in Edit mode
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

        // Optional helper: get a single user for editing
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
