using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.Models
{
    public class Permissions
    {
        [Key]
        public int Id { get; set; } // Id quyền

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } // Tên quyền (ViewBook, AddBook, EditBook)

        [MaxLength(255)]
        public string Description { get; set; } // Mô tả quyền

        // Liên kết với RolePermissions
        public virtual ICollection<RolePermissions> RolePermissions { get; set; } = new List<RolePermissions>();
    }
}
