using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TreeViewDemo.Data;

namespace TreeViewDemo.Controllers;

public class ServicesController(AppDbContext context) : Controller
{
    // GET
    public IActionResult LoginIdAvailability(int id, string loginId)
    {
        var existing = context.AppUsers.Any(m => m.LoginId == loginId && m.Id != id);
        return Json(!existing);
    }

    public async Task<IActionResult> MatchedTrees(string name, string parentName, string grandParentName)
    {
        var existingMatchingNodes = await context.Persons
            .Where(m => m.Name == name && m.Parent.Name == parentName && m.Parent.Parent.Name == grandParentName)
            .Select(m => m.Parent.Parent)
            .ToListAsync();

        var treeNames = existingMatchingNodes.Select(m => m.GetTreeName(context)).ToList();
        existingMatchingNodes.ForEach(m => m.ParentId = null);

        var jsonData = existingMatchingNodes.Select(context.LoadChildsRecursively).Select(m => m.BuildTree()).ToList();

        return Json(new { Nodes = jsonData, Trees = treeNames });
    }
}