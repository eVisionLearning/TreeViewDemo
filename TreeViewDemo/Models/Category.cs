namespace TreeViewDemo.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public int? ParentId { get; set; }
        public virtual Category Parent { get; set; }

        public List<Category> Childs { get; set; }
    }
}
