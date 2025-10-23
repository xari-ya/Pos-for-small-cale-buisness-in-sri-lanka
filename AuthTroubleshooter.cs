using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace billing_system
{
    internal static class AuthTroubleshooter
    {
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

        private static bool TableExists(SQLiteConnection conn, string name)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT 1 FROM sqlite_master WHERE type='table' AND name = @n;";
            cmd.Parameters.AddWithValue("@n", name);
            return cmd.ExecuteScalar() != null;
        }

        private static void PrintExistingTables(SQLiteConnection conn)
        {
            Log("Existing tables:");
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;";
            using var rd = cmd.ExecuteReader();
            while (rd.Read()) Log(" - " + rd.GetString(0));
        }

        private static void PrintTableColumns(SQLiteConnection conn, string table)
        {
            Log($"Columns in {table}:");
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"PRAGMA table_info({table});";
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
                Log($" - [{rd.GetInt32(0)}] {rd.GetString(1)} {rd.GetString(2)}");
        }

        private static int? GetUserId(SQLiteConnection conn, string username)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT user_id FROM Users WHERE username = @u;";
            cmd.Parameters.AddWithValue("@u", username);
            var val = cmd.ExecuteScalar();
            if (val == null || val == DBNull.Value) return null;
            return Convert.ToInt32(val);
        }

        private static void PrintHeader(string t)
        {
            Log(""); Log(new string('=', 56)); Log(t); Log(new string('=', 56));
        }

        private static void Log(string msg)
        {
            Console.WriteLine(msg);
            Debug.WriteLine(msg);
        }

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
        public static void CheckDatabase()
        {
            Console.WriteLine("\n--- DATABASE CHECK ---");

            // 1. Check the configured path
            string dbPath = AppConfig.DbPath;
            Console.WriteLine($"1. Configured DB Path: '{dbPath}'");

            // 2. Check if the file exists at that path
            if (File.Exists(dbPath))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("2. File Status: OK - File exists at the specified path.");
                Console.ResetColor();

                // 3. Try to connect and count products
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
