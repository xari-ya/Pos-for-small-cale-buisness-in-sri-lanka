using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace billing_system
{
    internal static class CategoryRepository
    {
        public sealed class CategoryItem
        {
            public string CategoryId { get; set; } = "";
            public string Name { get; set; } = "";
        }

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
