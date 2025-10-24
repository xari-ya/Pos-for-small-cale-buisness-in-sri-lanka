using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace billing_system
{
    /// <summary>
    /// Provides data access methods for product categories.
    /// </summary>
    /// <remarks>
    /// This static class is responsible for retrieving category information from the database.
    /// It is used by forms like <see cref="ProductDetailForm"/> to populate category selection lists.
    /// </remarks>
    internal static class CategoryRepository
    {
        /// <summary>
        /// Represents a single product category item.
        /// </summary>
        /// <remarks>
        /// This class serves as a simple data transfer object for category data.
        /// </remarks>
        public sealed class CategoryItem
        {
            /// <summary>
            /// Gets or sets the unique identifier for the category.
            /// </summary>
            public string CategoryId { get; set; } = "";

            /// <summary>
            /// Gets or sets the name of the category.
            /// </summary>
            public string Name { get; set; } = "";
        }

        /// <summary>
        /// Retrieves a list of all product categories from the database.
        /// </summary>
        /// <param name="conn">An open <see cref="SQLiteConnection"/> to the database.</param>
        /// <returns>A list of <see cref="CategoryItem"/> objects.</returns>
        /// <remarks>
        /// This method queries the 'Categories' table and returns all entries, ordered by name.
        /// </remarks>
        public static List<CategoryItem> GetAll(SQLiteConnection conn)
        {
            var list = new List<CategoryItem>();
            const string sql = @"SELECT category_id, name FROM Categories ORDER BY name;";
            using var cmd = new SQLiteCommand(sql, conn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new CategoryItem
                {
                    CategoryId = rd.GetString(0),
                    Name = rd.GetString(1)
                });
            }
            Console.WriteLine($"[CategoryRepository] Loaded {list.Count} categories.");
            return list;
        }
    }
}
