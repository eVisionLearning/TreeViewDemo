using System.ComponentModel.DataAnnotations;

namespace TreeViewDemo.Models;

public class AppUserLoginHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public AppUser User { get; set; }
    
    [StringLength(100)]
    public string Token { get; set; }
    public DateTime EntryTime { get; private set; } = DateTime.UtcNow;
}