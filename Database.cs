using System.Data.SQLite;
using System.IO;

namespace billing_system
{
    internal static class Database
    {
        private static readonly string _connStr =
            $"Data Source={AppConfig.DbPath};Version=3;Foreign Keys=True;";

        public static SQLiteConnection GetConnection()
        {
            if (!File.Exists(AppConfig.DbPath))
                SQLiteConnection.CreateFile(AppConfig.DbPath);
            return new SQLiteConnection(_connStr);
        }

        public static void Initialize()
        {
            using var conn = GetConnection();
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
PRAGMA foreign_keys = ON;

/* --- Users: UNCHANGED --- */
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

/* --- Categories: app-generated keys (no AUTOINCREMENT) --- */
CREATE TABLE IF NOT EXISTS Categories (
  category_id    TEXT PRIMARY KEY,          -- e.g., CAT-0001 or any string you generate
  name           TEXT NOT NULL UNIQUE,
  description    TEXT
);

/* --- Products: INTEGER PK you control (no AUTOINCREMENT) --- */
CREATE TABLE IF NOT EXISTS Products (
  product_id     INTEGER PRIMARY KEY,       -- you will set this from code
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

/* --- Invoices: BUSINESS PK --- */
CREATE TABLE IF NOT EXISTS Invoices (
  invoice_no     TEXT PRIMARY KEY,          -- yymmddNNNN
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

/* --- InvoiceItems: BUSINESS PK + FK by invoice_no --- */
CREATE TABLE IF NOT EXISTS InvoiceItems (
  invoice_item_id    TEXT PRIMARY KEY,      -- invoiceNo_seq e.g., 2409300001_0001
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

/* --- Payments: BUSINESS PK + FK by invoice_no --- */
CREATE TABLE IF NOT EXISTS Payments (
  payment_id    TEXT PRIMARY KEY,           -- P yymmdd NNNN e.g., P2409300001
  invoice_no    TEXT NOT NULL,
  amount        REAL NOT NULL CHECK (amount > 0),
  method        TEXT NOT NULL,
  received_at   TEXT NOT NULL DEFAULT (datetime('now')),
  ref_no        TEXT,
  notes         TEXT,
  FOREIGN KEY (invoice_no) REFERENCES Invoices(invoice_no) ON DELETE CASCADE
);
CREATE INDEX IF NOT EXISTS idx_payments_invoice_no ON Payments(invoice_no);

/* --- StockMovements: BUSINESS PK + invoice_no reference --- */
CREATE TABLE IF NOT EXISTS StockMovements (
  movement_id    TEXT PRIMARY KEY,          -- M yymmdd NNNN e.g., M2409300001
  product_id     INTEGER NOT NULL,
  movement_type  TEXT NOT NULL,             -- 'SALE','ADJUST','OPENING_STOCK','RETURN','PURCHASE'
  qty_change     REAL NOT NULL,             -- negative for sales
  reason         TEXT,
  ref_invoice_no TEXT,                       -- nullable
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
