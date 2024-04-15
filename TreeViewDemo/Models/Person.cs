using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace TreeViewDemo.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
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

        [NotMapped] public IFormFile Logo { get; set; }

        [NotMapped] public string TreeName { get; set; }

        [NotMapped] public string ParentName { get; set; }

        [NotMapped] public string ParentLogoUrl { get; set; }
        [NotMapped] public IFormFile ParentLogo { get; set; }

        [NotMapped] public string GrandParentName { get; set; }

        [NotMapped] public string GrandParentLogoUrl { get; set; }
        [NotMapped] public IFormFile GrandParentLogo { get; set; }
    }
}