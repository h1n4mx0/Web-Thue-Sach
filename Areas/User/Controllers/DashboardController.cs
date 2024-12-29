using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibraryManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; 
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LibraryManager.Areas.User.Models;



namespace LibraryManager.Areas.User.Controllers  // Đảm bảo namespace chính xác
{
    [Area("User")]
    [Authorize]
    public class DashboardController : BaseController
    {
        public DashboardController(MyDbContext context, ILogger<DashboardController> logger)
            : base(context, logger)
        {
        }
        private ProfileViewModel GetProfileViewModel()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return null;
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _db.Users
                .Include(u => u.UserAccounts)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            return new ProfileViewModel
            {
                Id = user.Id,
                Username = user.UserAccounts.Username,
                Email = user.UserAccounts.Email,
                FullName = user.FullName,
                Phone = user.Phone,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
            };
        }

        public IActionResult Index()
        {
            var model = GetProfileViewModel();
            if (model == null)
            {
                return NotFound();
            }

            var rentalBooks = _db.Rentals
                .Include(r => r.UserAccount)
                .Include(r => r.Book)
                .Where(r => r.UserAccount.Id == model.Id)
                .OrderByDescending(r => r.RentalDate) 
                .Take(5) 
                .ToList();

            Console.WriteLine(rentalBooks);
            ViewBag.RentalBooks = rentalBooks;
            return View(model);
        }

        public IActionResult MyProfile()
        {
            var model = GetProfileViewModel();
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        public IActionResult EditProfile()
        {
            var model = GetProfileViewModel();
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        public async Task<IActionResult> MyRentals(string filter)
        {
            var model = GetProfileViewModel();
            IQueryable<Rentals> rentalsQuery = _db.Rentals
                                                    .Include(r => r.UserAccount)
                                                    .Include(r => r.Book)
                                                    .Where(r => r.UserAccount.Id == model.Id)
                                                    .AsQueryable();

            // Thực hiện lọc dữ liệu
            switch (filter)
            {
                case "last5":
                    rentalsQuery = rentalsQuery.OrderByDescending(r => r.RentalDate).Take(5);
                    break;
                case "last15days":
                    rentalsQuery = rentalsQuery.Where(r => r.RentalDate >= DateTime.Now.AddDays(-15));
                    break;
                case "last30days":
                    rentalsQuery = rentalsQuery.Where(r => r.RentalDate >= DateTime.Now.AddDays(-30));
                    break;
                case "last6months":
                    rentalsQuery = rentalsQuery.Where(r => r.RentalDate >= DateTime.Now.AddMonths(-6));
                    break;
                case "year2018":
                    rentalsQuery = rentalsQuery.Where(r => r.RentalDate.Year == 2018);
                    break;
                case "all":
                default:
                    rentalsQuery = rentalsQuery.OrderByDescending(r => r.RentalDate).Take(5);
                    break;
            }

            if (rentalsQuery != null && rentalsQuery.Any())
            {
                ViewBag.Rental = await rentalsQuery.ToListAsync();
            }
            else
            {
                ViewBag.Rental = new List<Rentals>();
            }
            return View();
        }

        public async Task<IActionResult> RentBook(string filter)
        {
            var model = GetProfileViewModel();
            IQueryable<Rentals> rentalsQuery = _db.Rentals
                                                    .Include(r => r.UserAccount)
                                                    .Include(r => r.Book)
                                                    .Where(r => r.UserAccount.Id == model.Id)
                                                    .AsQueryable();
            rentalsQuery = rentalsQuery.Where(r => r.RentalStatus == "Đang thuê");
            if (rentalsQuery != null && rentalsQuery.Any())
            {
                ViewBag.Rental = await rentalsQuery.ToListAsync();
            }
            else
            {
                ViewBag.Rental = new List<Rentals>();
            }
            return View();
        }
    }
}
