using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.Models
{
    public class Roles
    {
        [Key]
        public int Id { get; set; } // Id vai trò

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } // Tên vai trò (Admin, User, Manager)

        // Liên kết với RolePermissions
        public virtual ICollection<RolePermissions> RolePermissions { get; set; } = new List<RolePermissions>();

        // Liên kết với UserAccounts
        public virtual ICollection<UserAccounts> UserAccounts { get; set; } = new List<UserAccounts>();
    }
}
