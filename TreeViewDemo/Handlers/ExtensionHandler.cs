using Microsoft.EntityFrameworkCore;
using TreeViewDemo.Data;
using TreeViewDemo.Models;

namespace TreeViewDemo
{
    public static class ExtensionHandler
    {
        public static IQueryable<Person> FilteredPersons(this AppDbContext context)
        {
            return context.Persons.Where(m => m.UserId == context.GetLoggedInUserId);
        }

        public static List<Person> LoadChildsRecursively(this AppDbContext db, Person person)
        {
            var allChilds = new List<Person>();
            LoadChilds(person, db, ref allChilds);
            return allChilds;
        }

        private static void LoadChilds(Person person, AppDbContext db, ref List<Person> allChilds)
        {
            allChilds.Add(person);
            db.Entry(person).Collection(c => c.Childs).Load();
            if (person.Childs?.Count != 0)
            {
                foreach (var child in person.Childs)
                {
                    LoadChilds(child, db, ref allChilds);
                }
            }
        }

        public static void SaveAllChildrenWithUpdatedParentIds(this AppDbContext db, Person person)
        {
            // Load all children recursively
            var allChildren = db.LoadChildsRecursively(person);

            // Iterate through each child
            foreach (var child in allChildren)
            {
                // Update parent ID
                child.ParentId = person.Id;

                // Clear the navigation property to avoid conflicts during saving
                child.Parent = null;

                // If the entity is not already being tracked, mark it as Added
                if (db.Entry(child).State == EntityState.Detached)
                {
                    db.Persons.Add(child); // Assuming Persons is your DbSet<Person>
                }
            }

            // Save changes to the database
            db.SaveChanges();
        }



        public static KeyValuePair<int, string> GetTreeName(this Person person, AppDbContext db)
        {
            var treeName = db.AppUsers.Where(m => m.Id == person.UserId).Select(m => m.TreeName).FirstOrDefault();

            return new KeyValuePair<int, string>(person.Id, treeName);
        }

        // public static List<Person> LoadParentsRecursively(this AppDbContext db, Person person)
        // {
        //     var allParents = new List<Person>();
        //     LoadParents(person, db, ref allParents);
        //     return allParents;
        // }
        //
        // private static void LoadParents(Person person, AppDbContext db, ref List<Person> allParents)
        // {
        //     allParents.Add(person);
        //     if (person.ParentId == null) return;
        //     var parent = db.Persons.Find(person.ParentId);
        //     if (parent != null)
        //     {
        //         LoadParents(parent, db, ref allParents);
        //     }
        // }

        public static Dictionary<string, object> BuildTree(this IEnumerable<Person> persons)
        {
            var tree = new Dictionary<string, object>
            {
                {
                    "root",
                    new Dictionary<string, object>
                    {
                        { "value", "root" },
                        { "parent", null },
                        { "logoUrl", null },
                        { "maritalStatus", "Unmarried"},
                        {"gender", "male"}
                    }
                }
            };

            foreach (var person in persons)
            {
                var node = new Dictionary<string, object>
                {
                    { "id", person.Id },
                    { "value", person.Name },
                    { "logoUrl", person.PhotoUrl },
                    { "parent", person.ParentId.HasValue ? $"_{person.ParentId}" : "root" },
                    { "parentId", person.ParentId },
                    { "color", person.TextColor },
                    { "bgColor", person.BgColor },
                    { "gender", person.Gender.ToString() },
                    { "maritalStatus", person.MaritalStatus.ToString() },
                    {
                        "attrs",
                        new string[]
                            { person.Attribute1, person.Attribute2, person.Attribute3, person.Attribute4 }
                    }
                };

                tree.Add($"_{person.Id}", node);
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