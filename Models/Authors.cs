using System.Collections.Generic;

namespace LibraryManager.Models
{
    public partial class Authors
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }

        public virtual ICollection<Books> Books { get; set; } = new List<Books>();
    }
}
