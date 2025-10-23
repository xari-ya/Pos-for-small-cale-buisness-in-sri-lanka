// -----------------------------------------------------------------------------
// StockMovementRepository.cs (EXPLAINED)
// -----------------------------------------------------------------------------
using System;
using System.Data.SQLite;

namespace billing_system
{
    internal static class StockMovementRepository
    {
        // Create a movement ID like "M2510240001"
        private static string NextMovementId(SQLiteConnection conn, SQLiteTransaction tx)
        {
            string datePart = DateTime.Now.ToString("yyMMdd");
            string prefix = "M" + datePart;

            // Count existing for today and add 1 (simple + safe enough for single-user desktop)
            using var cmd = new SQLiteCommand(
                "SELECT COUNT(1) FROM StockMovements WHERE movement_id LIKE @pfx;", conn, tx);
            cmd.Parameters.AddWithValue("@pfx", prefix + "%");
            long count = (long)(cmd.ExecuteScalar() ?? 0L);
            long next = count + 1;
            string id = prefix + next.ToString("D4");
            return id;
        }

        // Inserts a stock movement (used for OPENING_STOCK on product add)
        public static string Insert(SQLiteConnection conn, SQLiteTransaction tx,
                                    long productId, string movementType,
                                    decimal qtyChange, string reason,
                                    string? refInvoiceNo)
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
