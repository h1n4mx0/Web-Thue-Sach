using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibraryManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LibraryManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _db;

        public HomeController(MyDbContext context, ILogger<HomeController> logger)
        {
            _db = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var randomBooks = _db.Books
                .Include(b => b.Author) 
                .Include(b => b.Category) 
                .OrderBy(r => Guid.NewGuid()) 
                .Take(3) 
                .ToList();

            ViewBag.Books = randomBooks;
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [Authorize]
        public IActionResult CategoryDetail(int id)
        {
            var category = _db.Categories.FirstOrDefault(c => c.Id == id);
            
            if (category == null)
            {
                return NotFound();
            }

            var books = _db.Books
                .Include(b => b.Author)
                .Where(b => b.CategoryId == id)
                .ToList();

            ViewBag.Books = books;
            ViewBag.CategoryName = category.Name;

            return View();
        }

        [Authorize]
        public IActionResult BookDetail(int id) 
        {
            var userId = GetCurrentUserId();
            var book = _db.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var relatedBooks = _db.Books
                .Include(b => b.Author) 
                .Include(b => b.Category)
                .Where(b => b.Id != id && 
                        (b.CategoryId == book.CategoryId || b.AuthorId == book.AuthorId))
                .Take(5)
                .ToList();

            ViewBag.Book = book;
            ViewBag.RelatedBooks = relatedBooks;
            ViewBag.UserId = userId;
            
            return View();
        }

        [HttpGet]
        public IActionResult Search(bool? author, bool? category, int? yearRange, string searchQuery)
        {
            // Base query
            var booksQuery = _db.Books
                                .Include(b => b.Author)
                                .Include(b => b.Category)
                                .AsQueryable();

            // Áp dụng filter 1 (Tìm kiếm theo tên tác giả)
            if (author.HasValue && author.Value && !string.IsNullOrWhiteSpace(searchQuery))
            {
                booksQuery = booksQuery.Where(b => b.Author.Name.Contains(searchQuery));
            }

            // Áp dụng filter 2 (Tìm kiếm theo danh mục)
            if (category.HasValue && category.Value && !string.IsNullOrWhiteSpace(searchQuery))
            {
                booksQuery = booksQuery.Where(b => b.Category.Name.Contains(searchQuery));
            }

            if (!author.HasValue && !category.HasValue) {
                booksQuery = booksQuery.Where(b => b.Title.Contains(searchQuery) || 
                                                    b.ISBN.Contains(searchQuery) ||
                                                    b.Summary.Contains(searchQuery) ||
                                                    b.RentalPrice.Contains(searchQuery) ||
                                                    b.Status.Contains(searchQuery) ||
                                                    b.Author.Name.Contains(searchQuery) || 
                                                    b.Category.Name.Contains(searchQuery));
            }

            var results = booksQuery.ToList();

            // Truyền kết quả sang View
            return View(results);
        }


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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
