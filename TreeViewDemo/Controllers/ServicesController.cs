using Microsoft.AspNetCore.Mvc;
using TreeViewDemo.Data;

namespace TreeViewDemo.Controllers;

public class ServicesController(AppDbContext context) : Controller
{
    // GET
    public IActionResult LoginIdAvailability(int id, string loginId)
    {
        var existing= context.AppUsers.Any(m => m.LoginId == loginId && m.Id != id);
        return Json(!existing);
    }
}