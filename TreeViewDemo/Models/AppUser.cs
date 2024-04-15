using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TreeViewDemo.Models;

public class AppUser
{
    public int Id { get; set; }
    
    [StringLength(50)]
    [Required(ErrorMessage = "Enter Login Id")]
    [Remote("LoginIdAvailability", "Services", ErrorMessage = "Login Id already exists. Try different.")]
    [Display(Name = "Login Id")]
    public string LoginId { get; set; }
    
    [StringLength(500)]
    [Required(ErrorMessage = "Enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public List<AppUserLoginHistory> Logins { get; set; }

    // public int? TreeId { get; set; }
    // public Person Tree { get; set; }
    
    [StringLength(100, ErrorMessage = "Maximum 100 characters allowed")]
    public string TreeName { get; set; }
}

public class AppUserViewModel : AppUser
{
    [Required(ErrorMessage = "Confirm your password")]
    [Compare("Password", ErrorMessage = "Both passwords not matched.")]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}

public class AppUserLoginModel
{
    [Required(ErrorMessage ="Login Id required")]
    public string LoginId { get; set; }
    [Required(ErrorMessage ="Password required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}