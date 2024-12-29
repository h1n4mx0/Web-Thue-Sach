using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using LibraryManager.Areas.User.Models;
using LibraryManager.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


public class BaseController : Controller
{
    public readonly ILogger<BaseController> _logger;
    public readonly MyDbContext _db;

    public BaseController(MyDbContext context, ILogger<BaseController> logger)
    {
        _db = context;
        _logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null)
        {
            var userId = int.Parse(userIdClaim.Value);
            var user = _db.Users
                .FirstOrDefault(u => u.Id == userId);
            var totalRentals = _db.Rentals.Count(r => r.UserAccountId == userId);
            var outRentals = _db.Rentals.Count(r => r.UserAccountId == userId && r.RentalStatus == "Hết hạn");

            ViewBag.TotalRentals = totalRentals;
            ViewBag.OutRentals = outRentals;
            ViewBag.FullName = user.FullName;
        }
        else
        {
            ViewBag.TotalRentals = 0; // Nếu user chưa đăng nhập
        }
    }
}
