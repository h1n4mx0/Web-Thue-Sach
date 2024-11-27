using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.Models
{
    public class LoginModel
    {
         private string _ReturnUrl;
        [Required(ErrorMessage = "Vui lòng nhập email hoặc tên người dùng.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl 
        { 
            get
            {
                return string.IsNullOrEmpty(_ReturnUrl) ? "/Home/Index" : _ReturnUrl;
            }
            set
            {
                _ReturnUrl = value;
            }
        }

    }
}
