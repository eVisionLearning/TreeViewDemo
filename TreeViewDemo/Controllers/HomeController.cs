using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TreeViewDemo.Data;
using TreeViewDemo.Models;

namespace TreeViewDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Demo()
        {
            return View();
        }

        public IActionResult Index(bool id = false)
        {
            if (!_context.Persons.Any() && !id)
            {
                return View();
            }

            if (id && !_context.Persons.Any())
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "DefaultData.sql");
                int r = _context.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(filePath));
                if (r == 0)
                {
                    var xx = 10;
                }
            }

            return RedirectToAction("Index", "Persons");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
