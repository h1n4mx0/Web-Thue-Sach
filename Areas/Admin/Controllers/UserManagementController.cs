using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManager.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Dùng để kiểm tra Auth


namespace LibraryManager.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Chỉ Admin hoặc Manager được phép truy cập

    public class UserManagementController : Controller
    {
        private readonly MyDbContext _db;

        public UserManagementController(MyDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Roles = _db.Roles.ToList(); 
            var users = _db.UserAccounts.Include(u => u.Role).ToList();
            return View(users);
        }
        

        [HttpGet]
        public IActionResult EditRole(string userId, string roleId)
        {
            try 
            {
                // Parse userId và roleId ngay từ đầu
                int userIdInt = int.Parse(userId);
                int roleIdInt = int.Parse(roleId);
                Console.WriteLine(userIdInt);
                Console.WriteLine(roleIdInt);
                // Sử dụng giá trị đã parse
                var user = _db.UserAccounts.FirstOrDefault(u => u.Id == userIdInt);
                if (user == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                var role = _db.Roles.FirstOrDefault(r => r.Id == roleIdInt);
                if (role == null)
                {
                    return Json(new { success = false, message = "Role not found" });
                }

                user.RoleId = roleIdInt;
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (FormatException)
            {
                return Json(new { success = false, message = "Invalid user ID or role ID format" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

    }

}
