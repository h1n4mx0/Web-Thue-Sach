using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.Models
{
    public class RegisterViewModel
    {
        // Thông tin cá nhân
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        [MaxLength(255)]
        public string FullName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        // Thông tin tài khoản
        [Required(ErrorMessage = "Vui lòng nhập tên người dùng.")]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [MaxLength(255)]
        public string Email { get; set; }
    }
}
