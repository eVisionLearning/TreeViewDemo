using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TreeViewDemo.Data;
using TreeViewDemo.Filters;
using TreeViewDemo.Models;

namespace TreeViewDemo.Controllers
{
    [Authorized]
    public class CategoriesController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

        // GET: Categories
        public async Task<IActionResult> Index(int? parentId)
        {
            if (parentId.HasValue)
            {
                var parent = await _context.FilteredCategories().FirstOrDefaultAsync(m => m.Id == parentId);
                if (parent == null) return RedirectToAction("Index");
                ViewBag.parent = parent;
            }

            var data = await _context.FilteredCategories().Where(m => m.ParentId == parentId)
                .Include(m => m.Parent)
                .Include(m => m.Childs).ToListAsync();
            return View(data);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Childs(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.FilteredCategories()
                .Include(m => m.Parent)
                .Include(m => m.Childs)
                .ToListAsync();

            var category = await _context.FilteredCategories()
                .Include(m => m.Parent)
                .Include(m => m.Childs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public async Task<IActionResult> Create(int? parentId, bool partial = false)
        {
            if (!parentId.HasValue)
            {
                return View();
            }

            var parent = await _context.FilteredCategories().FirstOrDefaultAsync(m => m.Id == parentId);
            ViewBag.parent = parent;
            ViewBag.partial = partial;
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View(category);
            category.UserId = _context.GetLoggedInUserId;
            if (_context.FilteredCategories().Any())
            {
                _context.Add(category);
            }
            else
            {
                var obj = new Category
                {
                    Name = category.GrandParentName,
                    UserId = category.UserId,
                    Status = true,
                    Childs = new()
                    {
                        new Category()
                        {
                            Name = category.ParentName,
                            Childs = new()
                            {
                                category
                            },
                            UserId = category.UserId,
                            Status = true,
                        }
                    }
                };

                _context.Add(obj);
                if (!string.IsNullOrEmpty(category.TreeName))
                {
                    var user = await _context.AppUsers.FirstOrDefaultAsync(m => m.Id == _context.GetLoggedInUserId);
                    user.TreeName = category.TreeName;
                }
            }


            await _context.SaveChangesAsync();
            return category.Partial
                ? RedirectToAction("TreeView")
                : RedirectToAction(nameof(Index), new { category.ParentId });
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id, bool p = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.FilteredCategories().FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            category.BgColor ??= "#ffffff";
            category.TextColor ??= "#000000";
            if (!category.ParentId.HasValue)
                category.TreeName = await _context.AppUsers.Where(m => m.Id == _context.GetLoggedInUserId)
                    .Select(m => m.TreeName).FirstAsync();

            if (!p) return View(category);
            category.Partial = true;
            return PartialView("~/Views/Categories/Edit.cshtml", category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!_context.FilteredCategories().Any(m => m.Id == category.Id)) return NotFound();
                    _context.Update(category);
                    _context.Entry(category).Property(m => m.ParentId).IsModified = false;
                    _context.Entry(category).Property(m => m.UserId).IsModified = false;
                    if (!string.IsNullOrEmpty(category.TreeName))
                    {
                        var user = await _context.AppUsers.Where(m => m.Id == _context.GetLoggedInUserId).FirstAsync();
                        user.TreeName = category.TreeName;
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return category.Partial
                    ? RedirectToAction("TreeView")
                    : RedirectToAction(nameof(Index), new { category.ParentId });
            }

            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.FilteredCategories()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.FilteredCategories().FirstOrDefaultAsync(m => m.Id == id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AccessAnonymous]
        public async Task<IActionResult> TreeView(int? id, string keyword, string parentName,
            string grandParentName)
        {
            ViewBag.keyword = keyword;
            ViewBag.parentName = parentName;
            ViewBag.grandParentName = grandParentName;
            keyword = keyword?.ToLower();
            parentName = parentName?.ToLower();
            grandParentName = grandParentName?.ToLower();

            if (string.IsNullOrEmpty(keyword) && _context.GetLoggedInUserId == 0)
            {
                return View(new List<Category> { });
            }

            var categories = await _context.Categories
                .Include(m => m.Parent)
                .Include(m => m.Childs).ThenInclude(m => m.Childs).ThenInclude(m => m.Childs)
                .Include(m => m.User)
                .ToListAsync();

            if (id > 0)
            {
                var data = categories.Where(m => m.UserId == id).ToList();
                if (data.Count == 0) return RedirectToAction("Index");
                if (data.All(m => m.UserId == _context.GetLoggedInUserId)) ViewBag.editMode = true;
                return View(data);
            }
            else
            {
                var query = categories.Where(m => true);
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = categories.Where(m => m.User.TreeName?.ToLower() == keyword);
                }
                else
                {
                    id ??= _context.GetLoggedInUserId;
                    if (id == 0) id = null;
                }

                var data = query.Where(m => !id.HasValue || m.UserId == id).ToList();
                if (!string.IsNullOrEmpty(parentName))
                {
                    var parent = data.FirstOrDefault(m => m.Name?.ToLower() == parentName && m.ParentId.HasValue);
                    if (parent != null && string.IsNullOrEmpty(grandParentName))
                    {
                        data = _context.LoadChildsRecursively(parent);
                        parent.ParentId = null;
                    }
                    else if (string.IsNullOrEmpty(grandParentName))
                    {
                        data = [];
                    }
                    //data = data.Any(m => m.Name == parentName && m.ParentId.HasValue) ? data : [];
                }

                if (!string.IsNullOrEmpty(grandParentName))
                {
                    var grandParent = data.Where(m => m.Name?.ToLower() == parentName && m.ParentId.HasValue)
                        .Select(m => m.Parent).FirstOrDefault();
                    if (grandParent != null && grandParent?.Name?.ToLower() == grandParentName)
                    {
                        data = _context.LoadChildsRecursively(grandParent);
                        grandParent.ParentId = null;
                    }
                    else data = [];
                }

                if (data.Count == 0) return RedirectToAction("Index");
                if (data.All(m => m.UserId == _context.GetLoggedInUserId)) ViewBag.editMode = true;
                return View(data);
            }
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}