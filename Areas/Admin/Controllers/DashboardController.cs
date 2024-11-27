using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibraryManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            return View();
        }
    }
}
