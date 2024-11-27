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
        public IActionResult RentalDetail(string ISBN)
        {
            // Lấy UserId từ Claims thay vì từ Session
            int userAccountId = GetCurrentUserId();

            // Kiểm tra xem UserAccountId có tồn tại trong bảng UserAccounts không
            var userAccountExists = _db.UserAccounts.Any(u => u.Id == userAccountId);
            if (!userAccountExists)
            {
                ViewBag.Message = "Tài khoản người dùng không tồn tại!";
                return View();
            }

            // Kiểm tra xem sách có tồn tại trong bảng Books không
            var bookExists = _db.Books.Any(b => b.ISBN == ISBN);
            if (!bookExists)
            {
                ViewBag.Message = "Sách không tồn tại!";
                return View();
            }

            // Kiểm tra xem sách đã được thuê bởi user này chưa
            var existingRental = _db.Rentals
                .FirstOrDefault(r => r.UserAccountId == userAccountId && r.ISBN == ISBN && r.RentalStatus == "Đang thuê");

            if (existingRental != null)
            {
                ViewBag.Message = "Bạn đã thuê sách này rồi!";
            }
            else
            {
                // Tạo một bản ghi thuê sách mới
                var newRental = new Rentals
                {
                    UserAccountId = userAccountId,
                    ISBN = ISBN,
                    RentalDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddMinutes(1), // Hoặc thời gian khác
                    RentalStatus = "Đang thuê"
                };

                // Lưu vào cơ sở dữ liệu
                _db.Rentals.Add(newRental);
                _db.SaveChanges();

                ViewBag.Message = "Thuê sách thành công!";
            }

            // Truyền dữ liệu sang View để hiển thị thông tin chi tiết
            ViewBag.UserAccountId = userAccountId;
            ViewBag.ISBN = ISBN;

            return View();
        }

        // Lấy UserId từ Claims
        private int GetCurrentUserId()
        {
            // Lấy UserId từ Claim
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                return int.Parse(userIdClaim.Value);
            }
            
            // Nếu không tìm thấy, ném ngoại lệ
            throw new InvalidOperationException("User ID not found in claims.");
        }
    }
}
