using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibraryManager.Models;
using LibraryManager.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization; 
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryManager.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Manager")] // Chỉ Admin hoặc Manager được phép truy cập
    public class BookManagementController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<BookManagementController> _logger;
        private readonly MyDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookManagementController(MyDbContext context, ILogger<BookManagementController> logger, IWebHostEnvironment webHostEnvironment,IMemoryCache cache)
        {
            _cache = cache;
            _db = context;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<int> GetTotalBooksCountAsync()
        {
            const string cacheKey = "BooksTotalCount";
            if (!_cache.TryGetValue(cacheKey, out int totalCount))
            {
                totalCount = await _db.Books.CountAsync();
                _cache.Set(cacheKey, totalCount, TimeSpan.FromMinutes(10)); // Cache trong 10 phút
            }
            return totalCount;
        }

        [HttpGet]
        public async Task<IActionResult> Show(int page = 1, int pageSize = 5)
        {
            try
            {
                var totalItems = await GetTotalBooksCountAsync();
                var books = await _db.Books
                    .Include(b => b.Author) 
                    .Include(b => b.Category) 
                    .Skip((page - 1) * pageSize) 
                    .Take(pageSize) 
                    .ToListAsync();

                var pagedResult = new PagedResult<Books>
                {
                    Items = books,
                    TotalItems = totalItems,
                    PageNumber = page,
                    PageSize = pageSize
                };
                return View(pagedResult); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching books");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }


        // GET: Books/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories.ToList();
            ViewBag.Authors = _db.Authors.ToList();
            return View();
        }

        // POST: Admin/BookManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.Authors = _db.Authors.ToList();
                ViewBag.Categories = _db.Categories.ToList();
                return View(model);
            }
            if (int.TryParse(model.RentalPrice, out int rentalPrice))
            {
                model.RentalPrice = $"{rentalPrice:N0} VNĐ"; 
            }
            else
            {
                ModelState.AddModelError("RentalPrice", "Giá thuê phải là số hợp lệ.");
                return View(model);
            }
            string fileName = null;
            if (model.CoverImage != null && model.CoverImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "bookImages");
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.CoverImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.CoverImage.CopyToAsync(fileStream);
                }
            }

            var book = new Books
            {
                Title = model.Title,
                AuthorId = model.AuthorId,
                CategoryId = model.CategoryId,
                ISBN = model.ISBN,
                PublishedYear = model.PublishedYear,
                Summary = model.Summary,
                CoverImage = "~/bookImages/" + fileName,
                RentalPrice = model.RentalPrice,
                ReadingDuration = model.ReadingDuration,
                Status = model.Status,
                Views = 0 // Giá trị mặc định
            };

            _db.Books.Add(book);
            await _db.SaveChangesAsync();
            return RedirectToAction("Show");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = _db.Authors.ToList();
                ViewBag.Categories = _db.Categories.ToList();
                return View(model);
            }

            var book = await _db.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            // Xử lý giá thuê
            if (int.TryParse(model.RentalPrice, out int rentalPrice))
            {
                book.RentalPrice = $"{rentalPrice:N0} VNĐ";
            }
            else
            {
                ModelState.AddModelError("RentalPrice", "Giá thuê phải là số hợp lệ.");
                ViewBag.Authors = _db.Authors.ToList();
                ViewBag.Categories = _db.Categories.ToList();
                return View(model);
            }


            // Map các trường cơ bản từ ViewModel sang Book
            book.Title = model.Title;
            book.AuthorId = model.AuthorId;
            book.CategoryId = model.CategoryId;
            book.ISBN = model.ISBN;
            book.PublishedYear = model.PublishedYear;
            book.Summary = model.Summary;
            book.ReadingDuration = model.ReadingDuration;
            book.Status = model.Status;

            // Xử lý upload ảnh mới nếu có
            if (model.CoverImage != null && model.CoverImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "bookImages");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Xóa ảnh cũ nếu có
                if (!string.IsNullOrEmpty(book.CoverImage))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, book.CoverImage.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                        catch
                        {
                            // Log lỗi nếu cần
                        }
                    }
                }

                // Lưu ảnh mới
                string fileExtension = Path.GetExtension(model.CoverImage.FileName);
                string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                string relativePath = Path.Combine("bookImages", uniqueFileName);
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.CoverImage.CopyToAsync(fileStream);
                }

                book.CoverImage = "/" + relativePath.Replace('\\', '/');
            }
            else
            {
                // Giữ nguyên ảnh cũ
                book.CoverImage = model.OldCoverImage;
            }

            try
            {
                _db.Update(book);
                await _db.SaveChangesAsync();
                return RedirectToAction("Show");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật sách: " + ex.Message);
                ViewBag.Authors = _db.Authors.ToList();
                ViewBag.Categories = _db.Categories.ToList();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var model = new EditViewModel
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                ISBN = book.ISBN,
                OldCoverImage = book.CoverImage,  // Lưu đường dẫn ảnh hiện tại
                PublishedYear = book.PublishedYear,
                Summary = book.Summary,
                RentalPrice = book.RentalPrice,
                ReadingDuration = book.ReadingDuration,
                Status = book.Status
            };

            ViewBag.Authors = _db.Authors.ToList();
            ViewBag.Categories = _db.Categories.ToList();
            return View(model);
        }


        private async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null) return null;

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "books");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/books/" + fileName;
        }

        private bool BookExists(int id)
        {
            return  _db.Books.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Console.WriteLine("da vao day");
            var book = _db.Books.Find(id); 

            if (book != null)
            {
                if (!string.IsNullOrEmpty(book.CoverImage))
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, book.CoverImage.TrimStart('~','/'));
                    Console.WriteLine(imagePath);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _db.Books.Remove(book); 
                _db.SaveChanges(); 
            }

            return RedirectToAction("Show"); 
        }


        // Advanced Search Action
        public async Task<IActionResult> AdvancedSearch(string query, int page = 1, int pageSize = 5)
        {
            var booksQuery = _db.Books
                                .Include(b => b.Author)
                                .Include(b => b.Category) 
                                .AsQueryable(); 

            if (!string.IsNullOrEmpty(query))
            {
                var pattern = new Regex(@"(\w+):([^\s]+)", RegexOptions.IgnoreCase);
                var matches = pattern.Matches(query);
                var processedKeys = new HashSet<string>();

                foreach (Match match in matches)
                {
                    var field = match.Groups[1].Value.ToLower();
                    var value = match.Groups[2].Value;

                    switch (field)
                    {
                        case "title":
                            booksQuery = booksQuery.Where(b => b.Title.Contains(value));
                            break;

                        case "isbn":
                            booksQuery = booksQuery.Where(b => b.ISBN.Contains(value));
                            break;

                        case "year":
                            if (value.Contains('-'))
                            {
                                var years = value.Split('-');
                                if (years.Length == 2 && int.TryParse(years[0], out int startYear) && int.TryParse(years[1], out int endYear))
                                {
                                    booksQuery = booksQuery.Where(b => b.PublishedYear >= startYear && b.PublishedYear <= endYear);
                                }
                            }
                            else if (int.TryParse(value, out int year))
                            {
                                booksQuery = booksQuery.Where(b => b.PublishedYear == year);
                            }
                            break;

                        case "author":
                            booksQuery = booksQuery.Where(b => b.Author.Name.Contains(value));
                            break;

                        case "category":
                            booksQuery = booksQuery.Where(b => b.Category.Name.Contains(value));
                            break;

                        default:
                            booksQuery = booksQuery.Where(b => b.Title.Contains(value) || b.Author.Name.Contains(value) || b.Category.Name.Contains(value));
                            break;
                    }

                    processedKeys.Add(match.Value);
                }

                var remainingKeywords = query.Split(' ')
                                            .Where(keyword => !processedKeys.Contains(keyword) && !keyword.Contains(":"))
                                            .ToList();

                foreach (var keyword in remainingKeywords)
                {
                    bool isNumeric = int.TryParse(keyword, out int year);
                    booksQuery = booksQuery.Where(b => b.Title.Contains(keyword) ||
                                                    (isNumeric && b.PublishedYear == year) ||
                                                    b.Author.Name.Contains(keyword) ||
                                                    b.Category.Name.Contains(keyword));
                }
            }

            // Phân trang
            var totalItems = await booksQuery.CountAsync();
            var books = await booksQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Tạo đối tượng phân trang
            var pagedResult = new PagedResult<Books>
            {
                Items = books,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize
            };

            return View("Show", pagedResult);
        }
    }
}
