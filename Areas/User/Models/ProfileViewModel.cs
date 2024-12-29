using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace LibraryManager.Areas.User.Models
{
    public class ProfileViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Password { get; set; }

    }
}