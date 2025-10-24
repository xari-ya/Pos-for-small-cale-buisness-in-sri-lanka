using System;
using System.Data.SQLite;

namespace billing_system
{
    /// <summary>
    /// Provides data access methods for product-related operations.
    /// </summary>
    /// <remarks>
    /// This static class handles all direct database interactions for the 'Products' table, including inserting,
    /// updating, soft-deleting, and retrieving product records. It is used by the <see cref="InventoryControl"/>
    /// and other parts of the application that need to manage product data.
    /// </remarks>
    internal static class ProductRepository
    {
        /// <summary>
        /// Inserts a new product into the database.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/>.</param>
        /// <param name="tx">An active <see cref="SQLiteTransaction"/>.</param>
        /// <param name="sku">The stock keeping unit (SKU) of the product.</param>
        /// <param name="name">The name of the product.</param>
        /// <param name="categoryId">The ID of the product's category.</param>
        /// <param name="unitPrice">The unit price of the product.</param>
        /// <param name="isActive">The active status of the product (1 for active, 0 for inactive).</param>
        /// <returns>The ID of the newly inserted product.</returns>
        public static long Insert(SQLiteConnection conn, SQLiteTransaction tx, string sku, string name, string categoryId, decimal unitPrice, int isActive)
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
            using var idCmd = new SQLiteCommand("SELECT last_insert_rowid();", conn, tx);
            var id = (long)(idCmd.ExecuteScalar() ?? 0L);
            Console.WriteLine($"[ProductRepository] INSERT OK sku={sku}, product_id={id}");
            return id;
        }

        /// <summary>
        /// Updates an existing product's details in the database.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/>.</param>
        /// <param name="tx">An optional <see cref="SQLiteTransaction"/>.</param>
        /// <param name="productId">The ID of the product to update.</param>
        /// <param name="sku">The new SKU for the product.</param>
        /// <param name="name">The new name for the product.</param>
        /// <param name="categoryId">The new category ID for the product.</param>
        /// <param name="unitPrice">The new unit price for the product.</param>
        /// <param name="isActive">The new active status for the product.</param>
        public static void Update(SQLiteConnection conn, SQLiteTransaction? tx, long productId, string sku, string name, string categoryId, decimal unitPrice, int isActive)
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

        /// <summary>
        /// Soft-deletes a product by setting its 'is_active' flag to 0.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/>.</param>
        /// <param name="tx">An optional <see cref="SQLiteTransaction"/>.</param>
        /// <param name="productId">The ID of the product to soft-delete.</param>
        /// <remarks>
        /// This approach preserves the product record in the database for historical and auditing purposes.
        /// </remarks>
        public static void SoftDelete(SQLiteConnection conn, SQLiteTransaction? tx, long productId)
        {
            const string sql = @"UPDATE Products SET is_active=0 WHERE product_id=@id;";
            using var cmd = new SQLiteCommand(sql, conn, tx);
            cmd.Parameters.AddWithValue("@id", productId);
            var n = cmd.ExecuteNonQuery();
            Console.WriteLine($"[ProductRepository] SOFT DELETE product_id={productId}, rows={n}");
        }

        /// <summary>
        /// Retrieves a product's details from the database by its SKU.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/>.</param>
        /// <param name="sku">The SKU of the product to retrieve.</param>
        /// <returns>A tuple containing the product's details, or <c>null</c> if not found.</returns>
        public static (long productId, string sku, string name, string? categoryId, decimal price, int isActive)? GetBySku(SQLiteConnection conn, string sku)
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
            var tuple = (rd.GetInt64(0), rd.GetString(1), rd.GetString(2), rd.IsDBNull(3) ? null : rd.GetString(3), Convert.ToDecimal(rd["unit_price"]), Convert.ToInt32(rd["is_active"]));
            return tuple;
        }

        /// <summary>
        /// Retrieves a product's details from the database by its ID.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/>.</param>
        /// <param name="productId">The ID of the product to retrieve.</param>
        /// <returns>A tuple containing the product's details, or <c>null</c> if not found.</returns>
        public static (long productId, string sku, string name, string? categoryId, decimal price, int isActive)? GetById(SQLiteConnection conn, long productId)
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
            var tuple = (rd.GetInt64(0), rd.GetString(1), rd.GetString(2), rd.IsDBNull(3) ? null : rd.GetString(3), Convert.ToDecimal(rd["unit_price"]), Convert.ToInt32(rd["is_active"]));
            return tuple;
        }
    }
}
