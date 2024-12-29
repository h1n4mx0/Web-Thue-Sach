using System;
using Microsoft.AspNetCore.Mvc;
using LibraryManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LibraryManager.Controllers
{
    public class RentalController : Controller
    {
        private readonly MyDbContext _db;

        public RentalController(MyDbContext db)
        {
            _db = db;
        }

        [Authorize]
        // Action nhận hai tham số userId và ISBN từ URL để tạo một bản ghi thuê sách
        [HttpPost]
        public JsonResult RentalDetail(string ISBN)
        {
            try
            {
                int userAccountId = GetCurrentUserId();

                // Kiểm tra xem sách đã được thuê chưa
                var existingRental = _db.Rentals
                    .FirstOrDefault(r => r.UserAccountId == userAccountId && r.ISBN == ISBN && r.RentalStatus == "Đang thuê");

                if (existingRental != null)
                {
                    return Json(new { success = false, message = "Bạn đã thuê sách này rồi!" });
                }

                // Tạo bản ghi thuê mới
                var newRental = new Rentals
                {
                    UserAccountId = userAccountId,
                    ISBN = ISBN,
                    RentalDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddMinutes(5),
                    RentalStatus = "Đang thuê"
                };

                _db.Rentals.Add(newRental);
                _db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }



        [HttpGet]
        public JsonResult GetRentedBooks()
        {
            try
            {
                int userAccountId = GetCurrentUserId();

                // Lấy danh sách các sách đang thuê
                var rentedBooks = _db.Rentals
                    .Where(r => r.UserAccountId == userAccountId && r.RentalStatus == "Đang thuê")
                    .Select(r => r.ISBN) // Chỉ lấy ISBN
                    .ToList();
                Console.WriteLine(rentedBooks[0]);
                return Json(new { success = true, rentedBooks });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }


        // Lấy UserId từ Claims
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                return int.Parse(userIdClaim.Value);
            }

            throw new InvalidOperationException("User ID không tìm thấy trong Claims.");
        }

    }
}
