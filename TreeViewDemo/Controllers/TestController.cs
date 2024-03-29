using Microsoft.AspNetCore.Mvc;

namespace TreeViewDemo.Controllers
{
    public class TestController : Controller
    {
        public IActionResult VueApp()
        {
            return View();
        }
    }
}
