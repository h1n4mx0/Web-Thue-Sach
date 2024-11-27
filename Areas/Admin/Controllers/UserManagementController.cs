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
        [ValidateAntiForgeryToken] 
        public IActionResult EditRole(string userId, string roleId)
        {
            Console.WriteLine("da vao day");
            Console.WriteLine(userId);
            
            var user = _db.UserAccounts.FirstOrDefault(u => u.Id == int.Parse(userId));
            if (user == null)
            {
                Console.WriteLine("User not found");
                return RedirectToAction("Index");
            }
            Console.WriteLine(roleId);
            var role = _db.Roles.FirstOrDefault(r => r.Id == int.Parse(roleId));
            if (role == null)
            {
                Console.WriteLine("role not found");
                return RedirectToAction("Index");
            }

            user.RoleId = int.Parse(roleId);
            _db.SaveChanges();

            TempData["SuccessMessage"] = $"Vai trò của người dùng {user.Username} đã được thay đổi thành {role.RoleName}.";

            return RedirectToAction("Index");
        }

    }

}
