using System.Data.SQLite;
using System.IO;

namespace billing_system
{
    /// <summary>
    /// Manages the application's database, including connection, schema creation, and initial data seeding.
    /// </summary>
    /// <remarks>
    /// This static class is responsible for all low-level database operations. It ensures the database file exists,
    /// provides connections, and sets up the required tables and indexes on first run.
    /// </remarks>
    internal static class Database
    {
        /// <summary>
        /// The connection string for the SQLite database.
        /// </summary>
        /// <remarks>
        /// This string is constructed using the database path from <see cref="AppConfig.DbPath"/>.
        /// It specifies the data source, SQLite version, and enables foreign key support.
        /// </remarks>
        private static readonly string _connStr =
            $"Data Source={AppConfig.DbPath};Version=3;Foreign Keys=True;";

        /// <summary>
        /// Gets a new connection to the SQLite database.
        /// </summary>
        /// <returns>A new <see cref="SQLiteConnection"/> object.</returns>
        /// <remarks>
        /// This method checks if the database file specified in <see cref="AppConfig.DbPath"/> exists.
        /// If it does not, the method creates it before establishing a connection.
        /// This ensures that repositories like <see cref="UserRepository"/> can always connect to a valid database file.
        /// </remarks>
        public static SQLiteConnection GetConnection()
        {
            if (!File.Exists(AppConfig.DbPath))
                SQLiteConnection.CreateFile(AppConfig.DbPath);
            return new SQLiteConnection(_connStr);
        }

        /// <summary>
        /// Initializes the database by creating the schema if it does not already exist.
        /// </summary>
        /// <remarks>
        /// This method connects to the database and executes a series of SQL commands to create all necessary tables
        /// (Users, Categories, Products, Invoices, etc.) and their indexes. It is designed to be run safely on application startup.
        /// After ensuring the schema is in place, it calls <see cref="SeedAdminIfMissing"/> to create a default administrator account.
        /// </remarks>
        public static void Initialize()
        {
            using var conn = GetConnection();
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Users (
  user_id        INTEGER PRIMARY KEY AUTOINCREMENT,
  username       TEXT NOT NULL UNIQUE,
  password_hash  BLOB NOT NULL,
  password_salt  BLOB NOT NULL,
  full_name      TEXT NOT NULL,
  role           TEXT NOT NULL DEFAULT 'Cashier',
  is_active      INTEGER NOT NULL DEFAULT 1,
  created_at     TEXT NOT NULL DEFAULT (datetime('now')),
  last_login     TEXT
);

CREATE TABLE IF NOT EXISTS Categories (
  category_id    TEXT PRIMARY KEY,
  name           TEXT NOT NULL UNIQUE,
  description    TEXT
);

CREATE TABLE IF NOT EXISTS Products (
  product_id     INTEGER PRIMARY KEY,
  sku            TEXT NOT NULL UNIQUE,
  name           TEXT NOT NULL,
  category_id    TEXT,
  unit_price     REAL NOT NULL CHECK (unit_price >= 0),
  is_active      INTEGER NOT NULL DEFAULT 1,
  created_at     TEXT NOT NULL DEFAULT (datetime('now')),
  FOREIGN KEY (category_id) REFERENCES Categories(category_id) ON UPDATE CASCADE
);
CREATE INDEX IF NOT EXISTS idx_products_name     ON Products(name);
CREATE INDEX IF NOT EXISTS idx_products_category ON Products(category_id);

CREATE TABLE IF NOT EXISTS Invoices (
  invoice_no     TEXT PRIMARY KEY,
  issued_at      TEXT NOT NULL DEFAULT (datetime('now')),
  subtotal       REAL NOT NULL DEFAULT 0,
  tax_total      REAL NOT NULL DEFAULT 0,
  discount_total REAL NOT NULL DEFAULT 0,
  grand_total    REAL NOT NULL DEFAULT 0,
  status         TEXT NOT NULL DEFAULT 'UNPAID',
  created_by     INTEGER NOT NULL,
  FOREIGN KEY (created_by) REFERENCES Users(user_id) ON UPDATE CASCADE
);
CREATE INDEX IF NOT EXISTS idx_invoices_status ON Invoices(status);

CREATE TABLE IF NOT EXISTS InvoiceItems (
  invoice_item_id    TEXT PRIMARY KEY,
  invoice_no         TEXT NOT NULL,
  product_id         INTEGER NOT NULL,
  qty                REAL NOT NULL CHECK (qty > 0),
  unit_price_at_sale REAL NOT NULL CHECK (unit_price_at_sale >= 0),
  discount_amount    REAL NOT NULL DEFAULT 0,
  tax_rate_percent   REAL NOT NULL DEFAULT 0,
  line_total         REAL NOT NULL DEFAULT 0,
  FOREIGN KEY (invoice_no) REFERENCES Invoices(invoice_no) ON DELETE CASCADE,
  FOREIGN KEY (product_id) REFERENCES Products(product_id)
);
CREATE INDEX IF NOT EXISTS idx_items_invoice_no ON InvoiceItems(invoice_no);
CREATE INDEX IF NOT EXISTS idx_items_product    ON InvoiceItems(product_id);

CREATE TABLE IF NOT EXISTS Payments (
  payment_id    TEXT PRIMARY KEY,
  invoice_no    TEXT NOT NULL,
  amount        REAL NOT NULL CHECK (amount > 0),
  method        TEXT NOT NULL,
  received_at   TEXT NOT NULL DEFAULT (datetime('now')),
  ref_no        TEXT,
  notes         TEXT,
  FOREIGN KEY (invoice_no) REFERENCES Invoices(invoice_no) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS idx_payments_invoice_no ON Payments(invoice_no);

CREATE TABLE IF NOT EXISTS StockMovements (
  movement_id    TEXT PRIMARY KEY,
  product_id     INTEGER NOT NULL,
  movement_type  TEXT NOT NULL,
  qty_change     REAL NOT NULL,
  reason         TEXT,
  ref_invoice_no TEXT,
  created_at     TEXT NOT NULL DEFAULT (datetime('now')),
  FOREIGN KEY (product_id)    REFERENCES Products(product_id),
  FOREIGN KEY (ref_invoice_no)REFERENCES Invoices(invoice_no) ON DELETE SET NULL
);
CREATE INDEX IF NOT EXISTS idx_stock_product       ON StockMovements(product_id);
CREATE INDEX IF NOT EXISTS idx_stock_ref_invoiceNo ON StockMovements(ref_invoice_no);
";
                cmd.ExecuteNonQuery();
            }

            SeedAdminIfMissing(conn);
        }

        /// <summary>
        /// Seeds the database with a default administrator account if no users exist.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/> to the database.</param>
        /// <remarks>
        /// This method first checks if the 'Users' table is empty. If it is, a new user with the username 'admin'
        /// and password 'admin' is created. The password is securely hashed using the <see cref="PasswordHasher"/> class.
        /// This ensures that the application has at least one administrative user upon first launch.
        /// </remarks>
        private static void SeedAdminIfMissing(SQLiteConnection conn)
        {
            using (var check = conn.CreateCommand())
            {
                check.CommandText = "SELECT COUNT(1) FROM Users;";
                var count = (long)check.ExecuteScalar();
                if (count > 0) return;
            }

            var (hash, salt) = PasswordHasher.HashPassword("admin");
            using var insert = conn.CreateCommand();
            insert.CommandText = @"
                INSERT INTO Users (username, password_hash, password_salt, full_name, role, is_active)
                VALUES (@u, @h, @s, @n, @r, 1);";
            insert.Parameters.AddWithValue("@u", "admin");
            insert.Parameters.AddWithValue("@h", hash);
            insert.Parameters.AddWithValue("@s", salt);
            insert.Parameters.AddWithValue("@n", "System Administrator");
            insert.Parameters.AddWithValue("@r", "Admin");
            insert.ExecuteNonQuery();
        }
    }
}
