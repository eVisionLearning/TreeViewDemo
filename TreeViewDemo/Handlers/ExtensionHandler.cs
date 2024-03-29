using Microsoft.EntityFrameworkCore;
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

        public static List<Category> LoadChildsRecursively(this AppDbContext db, Category category)
        {
            var allChilds = new List<Category>();
            LoadChilds(category, db, ref allChilds);
            return allChilds;
        }

        private static void LoadChilds(Category category, AppDbContext db, ref List<Category> allChilds)
        {
            allChilds.Add(category);
            db.Entry(category).Collection(c => c.Childs).Load();
            if (category.Childs?.Count != 0)
            {
                foreach (var child in category.Childs)
                {
                    LoadChilds(child, db, ref allChilds);
                }
            }
        }

        public static KeyValuePair<int, string> GetTreeName(this Category category, AppDbContext db)
        {
            var treeName = db.AppUsers.Where(m => m.Id == category.UserId).Select(m => m.TreeName).FirstOrDefault();

            return new KeyValuePair<int, string>(category.Id, treeName);
        }

        // public static List<Category> LoadParentsRecursively(this AppDbContext db, Category category)
        // {
        //     var allParents = new List<Category>();
        //     LoadParents(category, db, ref allParents);
        //     return allParents;
        // }
        //
        // private static void LoadParents(Category category, AppDbContext db, ref List<Category> allParents)
        // {
        //     allParents.Add(category);
        //     if (category.ParentId == null) return;
        //     var parent = db.Categories.Find(category.ParentId);
        //     if (parent != null)
        //     {
        //         LoadParents(parent, db, ref allParents);
        //     }
        // }

        public static Dictionary<string, object> BuildTree(this IEnumerable<Category> categories)
        {
            var tree = new Dictionary<string, object>
            {
                {
                    "root",
                    new Dictionary<string, object>
                    {
                        { "value", "root" },
                        { "parent", null },
                        { "logoUrl", null }
                    }
                }
            };

            foreach (var category in categories)
            {
                var node = new Dictionary<string, object>
                {
                    { "id", category.Id },
                    { "value", category.Name },
                    { "logoUrl", category.LogoUrl },
                    { "parent", category.ParentId.HasValue ? $"_{category.ParentId}" : "root" },
                    { "parentId", category.ParentId },
                    { "color", category.TextColor },
                    { "bgColor", category.BgColor },
                    {
                        "attrs",
                        new string[]
                            { category.Attribute1, category.Attribute2, category.Attribute3, category.Attribute4 }
                    }
                };

                tree.Add($"_{category.Id}", node);
            }

            return tree;
        }

        public static async Task<string> SaveAs(this IFormFile file, string directory, string fileName = null, string extension = null)
        {
            if (file is not { Length: > 0 }) return null;
            if (string.IsNullOrEmpty(fileName)) fileName = CoreHandler.GetInstance().GetUniqueFileName();
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", directory);
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            if (!string.IsNullOrEmpty(extension))
            {
                if (!extension.StartsWith(".")) extension = "." + extension;
            }
            var fileUrl = @"/" + directory + "/" + fileName + (extension ?? Path.GetExtension(file.FileName));
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + fileUrl;
            if (File.Exists(filePath)) File.Delete(filePath);
            var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            stream.Close();
            return fileUrl;
        }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request.Headers != null && request.Headers["X-Requested-With"].ToString().Equals("XMLHttpRequest", comparisonType: StringComparison.OrdinalIgnoreCase);
        }

        // public static void UpdateLogData(this AppDbContext _context)
        // {
        //     var entries = _context.ChangeTracker.Entries().ToList();
        //
        //     var now = DateTime.UtcNow;
        //
        //     foreach (var entry in entries.Where(m => m.State == EntityState.Added))
        //     {
        //         dynamic entity = entry.Entity;
        //         try { entity.DbEntryTime = now; } catch (Exception) { }
        //         try { entity.LastModifiedTime = now; } catch (Exception) { }
        //         // try { entity.EUId = _context.LoggedInUser?.Id ?? null; } catch (Exception) { }
        //         // try { entity.LMUId = _context.LoggedInUser?.Id ?? null; } catch (Exception) { }
        //     }
        //
        //
        //
        //     foreach (var entry in entries.Where(m => m.State == EntityState.Modified))
        //     {
        //         //foreach (var prop in entry.Entity.GetType().GetProperties().Where(m => m.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0))
        //         //{
        //         //    var type = prop.GetType();
        //         //    var newValue = entry.Property(prop.Name).CurrentValue;
        //         //    var oldValue = entry.Property(prop.Name).OriginalValue;
        //         //}
        //
        //         dynamic entity = entry.Entity;
        //         try
        //         {
        //             _context.Entry(entity).Property("Token").IsModified = false;
        //             if (entity.ChangeToken && !string.IsNullOrEmpty(entity.Token))
        //             {
        //                 _context.Entry(entity).Property("Token").IsModified = true;
        //             }
        //         }
        //         catch { }
        //         try { _context.Entry(entity).Property("DbEntryTime").IsModified = false; } catch { }
        //         try { entity.LastModifiedTime = now; } catch (Exception) { }
        //         try { entity.LMUId = _context.LoggedInUser?.Id ?? null; } catch (Exception) { }
        //     }
        //
        //     foreach (dynamic entry in entries.Where(m => m.State == EntityState.Deleted))
        //     {
        //         // For Log Purpose, If required
        //     }
        // }
    }
}