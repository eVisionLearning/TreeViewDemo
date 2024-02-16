using TreeViewDemo.Models;

namespace TreeViewDemo
{
    public static class ExtensionHadler
    {
        public static Dictionary<string, object> BuildTree(this IEnumerable<Category> categories)
        {
            var tree = new Dictionary<string, object>();
            tree.Add("root", new Dictionary<string, object>
                {
                    { "value", "root" },
                    { "parent", null }
                });

            foreach (var category in categories)
            {
                var node = new Dictionary<string, object>
                {
                    { "value", category.Name },
                    { "parent", category.ParentId.HasValue ? $"_{category.ParentId}" : "root" }
                };

                tree.Add($"_{category.Id}", node);
            }

            return tree;
        }

    }
}
