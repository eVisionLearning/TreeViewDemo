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
    public class PersonsController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

        // GET: persons
        public async Task<IActionResult> Index(int? parentId)
        {
            if (parentId.HasValue)
            {
                var parent = await _context.FilteredPersons().FirstOrDefaultAsync(m => m.Id == parentId);
                if (parent == null) return RedirectToAction("Index");
                ViewBag.parent = parent;
            }

            var data = await _context.FilteredPersons().Where(m => m.ParentId == parentId)
                .Include(m => m.Parent)
                .Include(m => m.Childs).ToListAsync();
            return View(data);
        }

        // GET: persons/Details/5
        public async Task<IActionResult> Childs(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persons = await _context.FilteredPersons()
                .Include(m => m.Parent)
                .Include(m => m.Childs)
                .ToListAsync();

            var person = await _context.FilteredPersons()
                .Include(m => m.Parent)
                .Include(m => m.Childs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: persons/Create
        public async Task<IActionResult> Create(int? parentId, bool partial = false)
        {
            if (!parentId.HasValue)
            {
                return View(new Person { GrandParentName = ".Net", ParentName = ".Net Core", Name = "Web App" });
            }

            var parent = await _context.FilteredPersons().FirstOrDefaultAsync(m => m.Id == parentId);
            ViewBag.parent = parent;
            ViewBag.partial = partial;
            return View();
        }

        // POST: persons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Person person)
        {
            if (!ModelState.IsValid) return View(person);
            person.PhotoUrl = person.Logo?.SaveAs("persons").Result;
            person.ParentLogoUrl = person.ParentLogo?.SaveAs("persons").Result;
            person.GrandParentLogoUrl = person.GrandParentLogo?.SaveAs("persons").Result;
            person.UserId = _context.GetLoggedInUserId;
            if (_context.FilteredPersons().Any())
            {
                _context.Add(person);
            }
            else
            {
                var obj = new Person
                {
                    Name = person.GrandParentName,
                    UserId = person.UserId,
                    Status = true,
                    PhotoUrl = person.GrandParentLogoUrl,
                    Childs = new()
                    {
                        new Person()
                        {
                            Name = person.ParentName,
                            Childs = new()
                            {
                                person
                            },
                            UserId = person.UserId,
                            Status = true,
                            PhotoUrl = person.ParentLogoUrl
                        }
                    }
                };

                _context.Add(obj);
                if (!string.IsNullOrEmpty(person.TreeName))
                {
                    var user = await _context.AppUsers.FirstOrDefaultAsync(m => m.Id == _context.GetLoggedInUserId);
                    user.TreeName = person.TreeName;
                }
            }


            await _context.SaveChangesAsync();
            return person.Partial
                ? RedirectToAction("TreeView")
                : RedirectToAction(nameof(Index), new { person.ParentId });
        }

        // GET: persons/Edit/5
        public async Task<IActionResult> Edit(int? id, bool p = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.FilteredPersons().FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            person.BgColor ??= "#ffffff";
            person.TextColor ??= "#000000";
            if (!person.ParentId.HasValue)
                person.TreeName = await _context.AppUsers.Where(m => m.Id == _context.GetLoggedInUserId)
                    .Select(m => m.TreeName).FirstAsync();

            if (!p) return View(person);
            person.Partial = true;
            return PartialView("~/Views/Persons/Edit.cshtml", person);
        }

        // POST: persons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!_context.FilteredPersons().Any(m => m.Id == person.Id)) return NotFound();
                    person.PhotoUrl = person.Logo?.SaveAs("persons").Result;
                    _context.Update(person);
                    if (string.IsNullOrEmpty(person.PhotoUrl))
                        _context.Entry(person).Property(m => m.PhotoUrl).IsModified = false;
                    _context.Entry(person).Property(m => m.ParentId).IsModified = false;
                    _context.Entry(person).Property(m => m.UserId).IsModified = false;

                    if (!string.IsNullOrEmpty(person.TreeName))
                    {
                        var user = await _context.AppUsers.Where(m => m.Id == _context.GetLoggedInUserId).FirstAsync();
                        user.TreeName = person.TreeName;
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!personExists(person.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return person.Partial
                    ? RedirectToAction("TreeView")
                    : RedirectToAction(nameof(Index), new { person.ParentId });
            }

            return View(person);
        }

        // GET: persons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.FilteredPersons()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: persons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.FilteredPersons().FirstOrDefaultAsync(m => m.Id == id);
            if (person != null)
            {
                _context.Persons.Remove(person);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AccessAnonymous]
        public async Task<IActionResult> TreeView(string keyword, string firstParentName, string secondParentName, string thirdParentName)
        {
            ViewBag.keyword = keyword;
            ViewBag.firstParentName = firstParentName;
            ViewBag.secondParentName = secondParentName;
            ViewBag.thirdParentName = thirdParentName;
            keyword = keyword?.ToLower();
            firstParentName = firstParentName?.ToLower();
            secondParentName = secondParentName?.ToLower();
            thirdParentName = thirdParentName?.ToLower();

            if (string.IsNullOrEmpty(keyword) && _context.GetLoggedInUserId == 0)
            {
                return View(new List<Person> { });
            }

            var query = _context.Persons.AsQueryable();
            query = !string.IsNullOrEmpty(keyword)
                ? query.Where(m => m.User.TreeName == keyword)
                : query.Where(m => m.UserId == _context.GetLoggedInUserId);

            if (!query.Any()) return View(new List<Person> { });

            Person thirdParent = null;
            if (!string.IsNullOrEmpty(thirdParentName))
            {
                thirdParent = _context.Persons.Include(m => m.Parent).FirstOrDefault(m =>
                    m.Name == thirdParentName);
                if (thirdParent is not { ParentId: not null })
                    return View(new List<Person>());
            }

            Person secondParent = null;
            if (!string.IsNullOrEmpty(secondParentName))
            {
                secondParent = _context.Persons.Include(m => m.Parent).FirstOrDefault(m =>
                    m.Name == secondParentName);
                if (secondParent == null || (secondParent.ParentId != thirdParent?.Id && thirdParent != null))
                    return View(new List<Person>());
            }

            Person firstParent = null;
            if (!string.IsNullOrEmpty(firstParentName))
            {
                firstParent = _context.Persons.Include(m => m.Parent).FirstOrDefault(m =>
                    m.Name == firstParentName);
                if (firstParentName == null || (firstParent.ParentId != secondParent?.Id && secondParent != null))
                    return View(new List<Person>());
            }

            var treeParents = new List<Person> { thirdParent?.Parent ?? secondParent?.Parent ?? firstParent?.Parent }.Where(m => m != null).ToList();
            if (!treeParents.Any()) treeParents = await query.Where(m => !m.ParentId.HasValue).ToListAsync();

            var data = treeParents.SelectMany(_context.LoadChildsRecursively).ToList();
            if (data.All(m => m.UserId == _context.GetLoggedInUserId)) ViewBag.editMode = true;
            return View(data);
        }

        private bool personExists(int id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Merge(int id)
        {
            if (_context.Persons.Any(m => m.UserId == _context.GetLoggedInUserId))
            {
                return RedirectToAction("Index", "Home");
            }


            var person = context.Persons.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (person == null)
            {
                return null;
            }
            
            CoreHandler.GetInstance().LoadChildsRecursively(_context, person);
            //LoadChildsRecursively(person);
            //UpdateIdsRecursively(person);
            CoreHandler.GetInstance().UpdateIdsRecursively(_context, person);
            
            _context.GetLoggedInUser.TreeName = $"Merged - ${person.Name}";

            _context.Add(person);
            int r = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(TreeView));
        }

        //private void LoadChildsRecursively(person person)
        //{
        //    context.Entry(person).Collection(c => c.Childs).Load();

        //    if (person.Childs != null)
        //    {
        //        foreach (var child in person.Childs)
        //        {
        //            LoadChildsRecursively(child);
        //        }
        //    }
        //}

        //private void UpdateIdsRecursively(person person)
        //{
        //    person.ParentId = null;
        //    person.Id = 0;
        //    person.UserId = _context.GetLoggedInUserId;

        //    if (person.Childs != null)
        //    {
        //        foreach (var child in person.Childs)
        //        {
        //            UpdateIdsRecursively(child);
        //        }
        //    }
        //}
    }
}