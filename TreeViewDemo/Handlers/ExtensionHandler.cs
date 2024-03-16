using TreeViewDemo.Data;
using TreeViewDemo.Models;

namespace TreeViewDemo
{
    public static class ExtensionHandler
    {
        public static IQueryable<Category> FilteredCategories(this AppDbContext context)
        {
            return context.Categories.Where(m => m.UserId == context.GetLoggedInUserId);
        }
        public static Dictionary<string, object> BuildTree(this IEnumerable<Category> categories)
        {
            var tree = new Dictionary<string, object>
            {
                {
                    "root",
                    new Dictionary<string, object>
                    {
                        { "value", "root" },
                        { "parent", null }
                    }
                }
            };

            foreach (var category in categories)
            {
                var node = new Dictionary<string, object>
                {
                    { "id", category.Id },
                    { "value", category.Name },
                    { "parent", category.ParentId.HasValue ? $"_{category.ParentId}" : "root" },
                    { "parentId", category.ParentId },
                    { "color", category.TextColor },
                    { "bgColor", category.BgColor },
                    { "attrs" , new string[] { category.Attribute1,  category.Attribute2, category.Attribute3, category.Attribute4 } }
                };

                tree.Add($"_{category.Id}", node);
            }

            return tree;
        }

    }
}
