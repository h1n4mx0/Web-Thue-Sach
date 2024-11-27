using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManager.Models
{
    public partial class Rentals
    {
        [Key]
        public int Id { get; set; }  

        [Required]
        public int UserAccountId { get; set; } 

        [Required]
        public string ISBN { get; set; }  

        [Required]
        [DataType(DataType.Date)]
        public DateTime RentalDate { get; set; }  


        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }  

        [Required]
        [Column(TypeName = "enum('Đang thuê', 'Hết hạn')")]
        public string RentalStatus { get; set; } = "Đang thuê";  

        // Navigation properties
        [ForeignKey("UserAccountId")]
        public virtual UserAccounts UserAccount { get; set; }  

        [ForeignKey("ISBN")]
        public virtual Books Book { get; set; }  
    }
}
