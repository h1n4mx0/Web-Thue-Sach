using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LibraryManager.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManager.Services;
using System;
using System.Collections.Generic;

namespace LibraryManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDbContext _db;
        private readonly RentalService _rentalService;

        public AccountController(MyDbContext context, RentalService rentalService)
        {
            _db = context ?? throw new ArgumentNullException(nameof(context));
            _rentalService = rentalService ?? throw new ArgumentNullException(nameof(rentalService));
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl ?? Url.Action("Index", "Home"));
            }

            var loginModel = new LoginModel { ReturnUrl = returnUrl };
            return View(loginModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var hashedPassword = GetMD5(model.Password);
            var user = _db.UserAccounts
                        .Include(u => u.Role)
                        .FirstOrDefault(s => s.Username == model.UserName && s.Password == hashedPassword);
            Console.WriteLine(user.Role.RoleName);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.RoleName), // Use role name, assuming Role is navigational property
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                foreach (var claim in claims){
                    if (claim.Type == ClaimTypes.Role){
                        Console.WriteLine(claim.Value);
                    }
                }
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = model.RememberLogin
                });


                return Redirect(model.ReturnUrl ?? Url.Action("Index", "Home"));
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _db.UserAccounts.AnyAsync(s => s.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email is already in use.");
                return View(model);
            }

            if (await _db.UserAccounts.AnyAsync(s => s.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username is already in use.");
                return View(model);
            }

            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var user = new Users
                {
                    FullName = model.FullName,
                    DateOfBirth = model.DateOfBirth,
                    Address = model.Address,
                    Phone = model.Phone,
                    CreatedAt = DateTime.Now
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                var defaultRole = await _db.Roles.FirstOrDefaultAsync(r => r.RoleName == "User");
                if (defaultRole == null)
                {
                    ModelState.AddModelError("", "Default role not found. Please ensure roles are seeded in the database.");
                    return View(model);
                }
                var account = new UserAccounts
                {
                    UserId = user.Id,
                    Username = model.Username,
                    Password = GetMD5(model.Password),
                    Email = model.Email,
                    CreatedAt = DateTime.Now,
                    RoleId = defaultRole.Id
                };

                _db.UserAccounts.Add(account);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("Error saving data: " + ex.Message);
                ModelState.AddModelError("", "An error occurred while saving data. Please try again.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public static string GetMD5(string str)
        {
            using var md5 = MD5.Create();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            var byte2String = new StringBuilder();
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String.Append(targetData[i].ToString("x2"));
            }
            return byte2String.ToString();
        }
    }
}
