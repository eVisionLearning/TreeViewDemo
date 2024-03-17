﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace TreeViewDemo.Models
{
    public class Category
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
        public virtual Category Parent { get; set; }
        
        [NotMapped]
        public bool Partial { get; set; }
        public List<Category> Childs { get; set; }

        public int UserId { get; set; }
        public virtual AppUser User { get; set; }

        [NotMapped]
        public string TreeName { get; set; }
        
        [NotMapped]
        public string ParentName { get; set; }
        
        [NotMapped]
        public string GrandParentName { get; set; }
    }
}
