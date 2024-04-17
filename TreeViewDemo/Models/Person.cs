using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreeViewDemo.Models
{
    public class Person
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Enter person's name.")]
        public string Name { get; set; }
        public string LastName { get; set; }
        
        public string SpouseName { get; set; }
        public bool Status { get; set; }

        public string TextColor { get; set; }
        public string BgColor { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public string Attribute4 { get; set; }
        public int? ParentId { get; set; }
        public virtual Person Parent { get; set; }

        [NotMapped] public bool Partial { get; set; }
        public List<Person> Childs { get; set; }

        public int UserId { get; set; }
        public virtual AppUser User { get; set; }

        public string PhotoUrl { get; set; }
        
        [Required(ErrorMessage = "Choose Marital Status")]
        public MaritalStatus MaritalStatus { get; set; }
        
        [Required(ErrorMessage = "Choose Gender")]
        public Gender Gender { get; set; }

        [NotMapped] public IFormFile Photo { get; set; }

        [NotMapped] public string TreeName { get; set; }

        [NotMapped] public string ParentName { get; set; }

        [NotMapped] public string ParentLogoUrl { get; set; }
        [NotMapped] public IFormFile ParentLogo { get; set; }

        [NotMapped] public string GrandParentName { get; set; }

        [NotMapped] public string GrandParentLogoUrl { get; set; }
        [NotMapped] public IFormFile GrandParentLogo { get; set; }
    }

    public enum MaritalStatus
    {
        Unmarried = 0,
        Married = 1
    }

    public enum Gender
    {
        Female = 0,
        Male = 1
    }
}