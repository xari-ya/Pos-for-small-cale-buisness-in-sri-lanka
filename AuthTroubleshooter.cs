using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace billing_system
{
    /// <summary>
    /// Provides a set of diagnostic tools for troubleshooting authentication issues.
    /// </summary>
    /// <remarks>
    /// This static utility class helps developers diagnose problems related to database connectivity, user existence, and password verification.
    /// It is typically invoked from the <see cref="Login"/> form when an authentication attempt fails, logging detailed information to the Console and Debug outputs.
    /// </remarks>
    internal static class AuthTroubleshooter
    {
        /// <summary>
        /// Runs a basic set of diagnostic checks for a given username.
        /// </summary>
        /// <param name="username">The username to check for.</param>
        /// <remarks>
        /// This method performs the following checks:
        /// 1. Verifies that the database file exists at the path specified in <see cref="AppConfig.DbPath"/>.
        /// 2. Confirms that the 'Users' table exists within the database.
        /// 3. Checks if a user with the specified username exists in the 'Users' table.
        /// </remarks>
        public static void RunBasic(string username)
        {
            PrintHeader("AUTH TROUBLESHOOTER (Basic)");

            Log($"DbPath: {AppConfig.DbPath}");
            bool dbExists = File.Exists(AppConfig.DbPath);
            Log($"DB exists: {dbExists}");
            if (!dbExists) return;

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                bool usersTable = TableExists(conn, "Users");
                Log($"Users table exists: {usersTable}");
                if (!usersTable) { PrintExistingTables(conn); return; }

                PrintTableColumns(conn, "Users");

                int? userId = GetUserId(conn, username);
                Log($"User '{username}' exists: {userId.HasValue} {(userId.HasValue ? $"(user_id={userId.Value})" : "")}");
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }

        /// <summary>
        /// Runs a comprehensive set of diagnostic checks, including password verification.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <param name="password">The password to verify.</param>
        /// <remarks>
        /// This method first calls <see cref="RunBasic"/> to perform initial checks. If those succeed,
        /// it retrieves the user's stored hash and salt from the database and uses the <see cref="PasswordHasher"/>
        /// to verify if the provided password is correct.
        /// </remarks>
        public static void RunFull(string username, string password)
        {
            PrintHeader("AUTH TROUBLESHOOTER (Full)");
            RunBasic(username);
            if (!File.Exists(AppConfig.DbPath)) return;

            try
            {
                using var conn = Database.GetConnection();
                conn.Open();
                if (!TableExists(conn, "Users")) return;

                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT password_hash, password_salt
                    FROM Users
                    WHERE username = @u;";
                cmd.Parameters.AddWithValue("@u", username);

                using var rd = cmd.ExecuteReader();
                if (!rd.Read()) { Log("Cannot verify: user not found."); return; }

                byte[] hash = (byte[])rd["password_hash"];
                byte[] salt = (byte[])rd["password_salt"];
                Log($"Stored hash length: {hash?.Length ?? 0}; salt length: {salt?.Length ?? 0}");

                bool ok = PasswordHasher.Verify(password, hash, salt);
                Log($"Password verification result: {ok}");
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
        }

        /// <summary>
        /// Checks if a table with a given name exists in the database.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/>.</param>
        /// <param name="name">The name of the table to check.</param>
        /// <returns><c>true</c> if the table exists; otherwise, <c>false</c>.</returns>
        private static bool TableExists(SQLiteConnection conn, string name)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT 1 FROM sqlite_master WHERE type='table' AND name = @n;";
            cmd.Parameters.AddWithValue("@n", name);
            return cmd.ExecuteScalar() != null;
        }

        /// <summary>
        /// Prints a list of all tables in the database to the log.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/>.</param>
        private static void PrintExistingTables(SQLiteConnection conn)
        {
            Log("Existing tables:");
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;";
            using var rd = cmd.ExecuteReader();
            while (rd.Read()) Log(" - " + rd.GetString(0));
        }

        /// <summary>
        /// Prints the column definitions for a given table to the log.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/>.</param>
        /// <param name="table">The name of the table to inspect.</param>
        private static void PrintTableColumns(SQLiteConnection conn, string table)
        {
            Log($"Columns in {table}:");
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"PRAGMA table_info({table});";
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                Log($" - [{rd.GetInt32(0)}] {rd.GetString(1)} {rd.GetString(2)}");
        }

        /// <summary>
        /// Gets the user ID for a given username.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/>.</param>
        /// <param name="username">The username to look up.</param>
        /// <returns>The user's ID, or <c>null</c> if not found.</returns>
        private static int? GetUserId(SQLiteConnection conn, string username)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT user_id FROM Users WHERE username = @u;";
            cmd.Parameters.AddWithValue("@u", username);
            var val = cmd.ExecuteScalar();
            if (val == null || val == DBNull.Value) return null;
            return Convert.ToInt32(val);
        }

        /// <summary>
        /// Prints a formatted header to the log.
        /// </summary>
        /// <param name="t">The text to display in the header.</param>
        private static void PrintHeader(string t)
        {
            Log(""); Log(new string('=', 56)); Log(t); Log(new string('=', 56));
        }

        /// <summary>
        /// Logs a message to both the Console and Debug outputs.
        /// </summary>
        /// <param name="msg">The message to log.</param>
        private static void Log(string msg)
        {
            Console.WriteLine(msg);
            Debug.WriteLine(msg);
        }

        /// <summary>
        /// Checks and logs the status of the current user session.
        /// </summary>
        /// <param name="context">A string describing the context in which the check is being performed (e.g., "After successful login").</param>
        /// <remarks>
        /// This method inspects <see cref="AppSession.CurrentUser"/> to verify if a user is logged in and prints the result,
        /// including the user's ID and name if available, to the console with color-coded status.
        /// </remarks>
        public static void CheckCurrentUser(string context)
        {
            Console.WriteLine($"--- SESSION CHECK ({context}) ---");
            if (AppSession.CurrentUser != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"SUCCESS: AppSession.CurrentUser is set.");
                Console.WriteLine($"  - UserID: {AppSession.CurrentUser.UserId}");
                Console.WriteLine($"  - FullName: {AppSession.CurrentUser.FullName}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: AppSession.CurrentUser is NULL!");
                Console.ResetColor();
            }
            Console.WriteLine("-------------------------------------");
        }

        /// <summary>
        /// Performs a series of checks on the database file and its contents.
        /// </summary>
        /// <remarks>
        /// This method checks the configured database path, verifies the file's existence, and attempts to connect
        /// and query the 'Products' table to ensure the database is properly set up and accessible.
        /// It provides helpful, color-coded feedback to the console for quick diagnosis.
        /// </remarks>
        public static void CheckDatabase()
        {
            Console.WriteLine("\n--- DATABASE CHECK ---");

            string dbPath = AppConfig.DbPath;
            Console.WriteLine($"1. Configured DB Path: '{dbPath}'");

            if (File.Exists(dbPath))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("2. File Status: OK - File exists at the specified path.");
                Console.ResetColor();

                try
                {
                    using (var conn = Database.GetConnection())
                    {
                        conn.Open();
                        using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Products", conn))
                        {
                            long productCount = (long)cmd.ExecuteScalar();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("3. Data Status: OK - Successfully connected and found");
                            Console.WriteLine($"   {productCount} products in the 'Products' table.");
                            Console.ResetColor();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("3. Data Status: FAILED - Could not connect or query the database.");
                    Console.WriteLine($"   Error: {ex.Message}");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("2. File Status: FAILED - File does NOT exist at the specified path!");
                Console.WriteLine("   >> FIX: In Solution Explorer, right-click 'bims.db', go to Properties,");
                Console.WriteLine("   >> and set 'Copy to Output Directory' to 'Copy if newer'.");
                Console.ResetColor();
            }
            Console.WriteLine("------------------------\n");
        }
    }
}
