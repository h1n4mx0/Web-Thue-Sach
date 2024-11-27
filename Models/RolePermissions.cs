using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManager.Models
{
    public class RolePermissions
    {
        [Key]
        public int Id { get; set; } // Id mối liên kết

        [Required]
        public int RoleId { get; set; } // Khóa ngoại Role

        [Required]
        public int PermissionId { get; set; } // Khóa ngoại Permission

        [ForeignKey("RoleId")]
        public virtual Roles Role { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permissions Permission { get; set; }
    }
}
