using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManager.Models
{
    public partial class UserAccounts
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; } 
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public int RoleId { get; set; } 

        [ForeignKey("RoleId")]
        public virtual Roles Role { get; set; }

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
        public virtual ICollection<Rentals> Rentals { get; set; } = new List<Rentals>();
    }
}
