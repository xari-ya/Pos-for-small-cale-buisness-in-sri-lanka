using System;
using System.Data.SQLite;

namespace billing_system
{
    /// <summary>
    /// Provides data access methods for stock movement operations.
    /// </summary>
    /// <remarks>
    /// This static class is responsible for recording all changes in product inventory levels in the 'StockMovements' table.
    /// It is used for actions such as recording initial stock, sales, returns, and manual adjustments.
    /// </remarks>
    internal static class StockMovementRepository
    {
        /// <summary>
        /// Generates a unique ID for a new stock movement.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/>.</param>
        /// <param name="tx">An active <see cref="SQLiteTransaction"/>.</param>
        /// <returns>A unique string identifier for the stock movement (e.g., "M2510240001").</returns>
        /// <remarks>
        /// The ID is generated based on the current date and a sequential number for that day.
        /// </remarks>
        private static string NextMovementId(SQLiteConnection conn, SQLiteTransaction tx)
        {
            string datePart = DateTime.Now.ToString("yyMMdd");
            string prefix = "M" + datePart;
            using var cmd = new SQLiteCommand("SELECT COUNT(1) FROM StockMovements WHERE movement_id LIKE @pfx;", conn, tx);
            cmd.Parameters.AddWithValue("@pfx", prefix + "%");
            long count = (long)(cmd.ExecuteScalar() ?? 0L);
            long next = count + 1;
            string id = prefix + next.ToString("D4");
            return id;
        }

        /// <summary>
        /// Inserts a new stock movement record into the database.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/>.</param>
        /// <param name="tx">An active <see cref="SQLiteTransaction"/>.</param>
        /// <param name="productId">The ID of the product for which the stock is changing.</param>
        /// <param name="movementType">The type of movement (e.g., "OPENING_STOCK", "SALE").</param>
        /// <param name="qtyChange">The change in quantity (positive for additions, negative for reductions).</param>
        /// <param name="reason">An optional reason for the movement.</param>
        /// <param name="refInvoiceNo">An optional reference to an invoice number.</param>
        /// <returns>The ID of the newly created stock movement record.</returns>
        /// <remarks>
        /// This method is called when a product's stock level changes, for example, when a new product is added with an initial quantity
        /// via the <see cref="InventoryControl"/>.
        /// </remarks>
        public static string Insert(SQLiteConnection conn, SQLiteTransaction tx, long productId, string movementType, decimal qtyChange, string reason, string? refInvoiceNo)
        {
            string movementId = NextMovementId(conn, tx);
            const string sql = @"
INSERT INTO StockMovements
(movement_id, product_id, movement_type, qty_change, reason, ref_invoice_no)
VALUES (@id, @pid, @type, @qty, @reason, @ref);";
            using var cmd = new SQLiteCommand(sql, conn, tx);
            cmd.Parameters.AddWithValue("@id", movementId);
            cmd.Parameters.AddWithValue("@pid", productId);
            cmd.Parameters.AddWithValue("@type", movementType);
            cmd.Parameters.AddWithValue("@qty", qtyChange);
            cmd.Parameters.AddWithValue("@reason", (object?)reason ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ref", (object?)refInvoiceNo ?? DBNull.Value);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"[StockMovementRepository] INSERT {movementType} id={movementId} pid={productId} qty={qtyChange}");
            return movementId;
        }
    }
}
