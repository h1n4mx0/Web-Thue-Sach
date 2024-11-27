using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibraryManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;  // Đảm bảo import đúng namespace

namespace LibraryManager.Areas.User.Controllers  // Đảm bảo namespace chính xác
{
    [Area("User")]  // Chỉ định tên của Area là "User"
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
            
            return View();
        }
    }
}
