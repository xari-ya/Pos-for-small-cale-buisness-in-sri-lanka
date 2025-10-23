// -----------------------------------------------------------------------------
// ProductRepository.cs  (EXPLAINED)
// -----------------------------------------------------------------------------
using System;
using System.Data.SQLite;

namespace billing_system
{
    internal static class ProductRepository
    {
        // Insert a product. Pass NULL for product_id so SQLite assigns it (rowid).
        // Returns the new product_id (long).
        public static long Insert(SQLiteConnection conn, SQLiteTransaction tx,
                                  string sku, string name, string categoryId,
                                  decimal unitPrice, int isActive)
        {
            const string sql = @"
INSERT INTO Products (product_id, sku, name, category_id, unit_price, is_active)
VALUES (NULL, @sku, @name, @category_id, @unit_price, @is_active);";

            using var cmd = new SQLiteCommand(sql, conn, tx);
            cmd.Parameters.AddWithValue("@sku", sku);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@category_id", (object?)categoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@unit_price", unitPrice);
            cmd.Parameters.AddWithValue("@is_active", isActive);
            cmd.ExecuteNonQuery();

            // last_insert_rowid() returns the rowid for the last insert on this connection
            using var idCmd = new SQLiteCommand("SELECT last_insert_rowid();", conn, tx);
            var id = (long)(idCmd.ExecuteScalar() ?? 0L);
            Console.WriteLine($"[ProductRepository] INSERT OK sku={sku}, product_id={id}");
            return id;
        }

        // Update fields that are editable in the form
        public static void Update(SQLiteConnection conn, SQLiteTransaction? tx,
                                  long productId, string sku, string name,
                                  string categoryId, decimal unitPrice, int isActive)
        {
            const string sql = @"
UPDATE Products
SET sku=@sku, name=@name, category_id=@category_id,
    unit_price=@unit_price, is_active=@is_active
WHERE product_id=@id;";

            using var cmd = new SQLiteCommand(sql, conn, tx);
            cmd.Parameters.AddWithValue("@sku", sku);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@category_id", (object?)categoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@unit_price", unitPrice);
            cmd.Parameters.AddWithValue("@is_active", isActive);
            cmd.Parameters.AddWithValue("@id", productId);
            var n = cmd.ExecuteNonQuery();
            Console.WriteLine($"[ProductRepository] UPDATE product_id={productId}, rows={n}");
        }

        // Soft delete: keep history/audit intact
        public static void SoftDelete(SQLiteConnection conn, SQLiteTransaction? tx, long productId)
        {
            const string sql = @"UPDATE Products SET is_active=0 WHERE product_id=@id;";
            using var cmd = new SQLiteCommand(sql, conn, tx);
            cmd.Parameters.AddWithValue("@id", productId);
            var n = cmd.ExecuteNonQuery();
            Console.WriteLine($"[ProductRepository] SOFT DELETE product_id={productId}, rows={n}");
        }

        // Convenience lookups
        public static (long productId, string sku, string name, string? categoryId, decimal price, int isActive)?
            GetBySku(SQLiteConnection conn, string sku)
        {
            const string sql = @"
SELECT product_id, sku, name, category_id, unit_price, is_active
FROM Products
WHERE sku=@sku
LIMIT 1;";

            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sku", sku);
            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;

            var tuple = (
                rd.GetInt64(0),
                rd.GetString(1),
                rd.GetString(2),
                rd.IsDBNull(3) ? null : rd.GetString(3),
                Convert.ToDecimal(rd["unit_price"]),
                Convert.ToInt32(rd["is_active"])
            );
            return tuple;
        }

        public static (long productId, string sku, string name, string? categoryId, decimal price, int isActive)?
            GetById(SQLiteConnection conn, long productId)
        {
            const string sql = @"
SELECT product_id, sku, name, category_id, unit_price, is_active
FROM Products
WHERE product_id=@id
LIMIT 1;";

            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", productId);
            using var rd = cmd.ExecuteReader();
            if (!rd.Read()) return null;

            var tuple = (
                rd.GetInt64(0),
                rd.GetString(1),
                rd.GetString(2),
                rd.IsDBNull(3) ? null : rd.GetString(3),
                Convert.ToDecimal(rd["unit_price"]),
                Convert.ToInt32(rd["is_active"])
            );
            return tuple;
        }
    }
}
