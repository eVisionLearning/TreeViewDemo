using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TreeViewDemo.Data;
using TreeViewDemo.Models;

namespace TreeViewDemo.Controllers
{
    public class CategoriesController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

        // GET: Categories
        public async Task<IActionResult> Index(int? parentId)
        {
            if (parentId.HasValue)
            {
                var parent = _context.Categories.Find(parentId);
                ViewBag.parent = parent;
            }

            return View(await _context.Categories.Where(m => m.ParentId == parentId).Include(m => m.Parent)
                .Include(m => m.Childs).ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Childs(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories.Include(m => m.Parent).Include(m => m.Childs).ToListAsync();

            var category = await _context.Categories
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
            if (!parentId.HasValue) return View();
            var parent = await _context.Categories.FindAsync(parentId);
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
            _context.Add(category);
            await _context.SaveChangesAsync();
            return category.Partial ? RedirectToAction("TreeView") : RedirectToAction(nameof(Index), new { category.ParentId });
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id, bool p = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            
            category.BgColor ??= "#ffffff";
            category.TextColor ??= "#000000";

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
                    _context.Update(category);
                    _context.Entry(category).Property(m => m.ParentId).IsModified = false;
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

            var category = await _context.Categories
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
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> TreeView()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}