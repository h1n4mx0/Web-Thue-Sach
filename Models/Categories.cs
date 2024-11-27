using System.Collections.Generic;

namespace LibraryManager.Models
{
    public partial class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Books> Books { get; set; } = new List<Books>();
    }
}
