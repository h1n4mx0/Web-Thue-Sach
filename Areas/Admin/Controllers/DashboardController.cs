using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibraryManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json; 
using Microsoft.AspNetCore.Authorization; // Dùng để kiểm tra Auth

namespace LibraryManager.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")] // Chỉ Admin hoặc Manager được phép truy cập
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly MyDbContext _db;

        public DashboardController(MyDbContext context, ILogger<DashboardController> logger)
        {
            _db = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            int totalBooks = _db.Books.Count();
            DateTime today = DateTime.Now;
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(7);

            int weeklyRentals = _db.Rentals
                .Where(r => r.RentalDate >= startOfWeek && r.RentalDate < endOfWeek)
                .Count();
            int totalSubscribers = _db.Users.Count();
            int completedBooks = _db.Books.Count(b => b.Status == "Đã hoàn thành");
            int inProgressBooks = _db.Books.Count(b => b.Status == "Đang cập nhật");
            var currentDate = DateTime.Now;

            var monthLabels = new List<string>();
            var userCounts = new List<int>();
            var rentalCounts = new List<int>();

            for (int i = 7; i >= 0; i--)
            {
                var date = currentDate.AddMonths(-i);
                var startOfMonth = new DateTime(date.Year, date.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1);

                // Đếm số lượt thuê trong tháng
                var rentalsInMonth = _db.Rentals
                    .Count(r => r.RentalDate >= startOfMonth && r.RentalDate < endOfMonth);

                monthLabels.Add(date.ToString("MMM").ToUpper());
                userCounts.Add(totalSubscribers);
                rentalCounts.Add(rentalsInMonth);
            }
            var recentRentals = _db.Rentals
                .Include(r => r.UserAccount)
                .Include(r => r.Book)
                .OrderByDescending(r => r.RentalDate)
                .Take(5)
                .Select(r => new
                {
                    UserName = r.UserAccount.Username,
                    BookImage = r.Book.CoverImage,
                    BookTitle = r.Book.Title,
                    Status = r.RentalStatus,
                    RentalDate = r.RentalDate,
                })
                .ToList();
            var recentBooks = _db.Books
                .OrderByDescending(r => r.Id)
                .Take(4)
                .Select(b => new
                {
                    Title = b.Title,
                    Image = b.CoverImage,
                    Description = b.Summary
                })
                .ToList();

            ViewBag.RecentBooks = recentBooks;
            ViewBag.RecentRentals = recentRentals;
            // Truyền dữ liệu vào ViewBag
            ViewBag.MonthLabels = JsonSerializer.Serialize(monthLabels);
            ViewBag.UserCounts = JsonSerializer.Serialize(userCounts);
            ViewBag.RentalCounts = JsonSerializer.Serialize(rentalCounts);
            ViewBag.CompletedBooks = completedBooks;
            ViewBag.InProgressBooks = inProgressBooks;                    
            ViewBag.TotalSubscribers = totalSubscribers;
            ViewBag.WeeklyRentals = weeklyRentals;
            ViewBag.TotalBooks = totalBooks;
            return View();
        }
    }
}
