using System;
using Microsoft.AspNetCore.Mvc;
using LibraryManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LibraryManager.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly MyDbContext _db;

        public BookController(MyDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Read(string ISBN)
        {
            Console.WriteLine(ISBN);
            if (string.IsNullOrEmpty(ISBN))
            {
                return NotFound("ISBN không hợp lệ.");
            }

            // Kiểm tra nếu người dùng đã thuê sách này
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); 
            var rentedBook = await _db.Rentals
                .AnyAsync(r => r.UserAccountId == userId && r.Book.ISBN == ISBN && r.RentalStatus == "Đang thuê");

            if (rentedBook)
            {
                // Lấy thông tin sách và nội dung từ cơ sở dữ liệu
                var bookContents = await _db.BookContents
                    .Include(bc => bc.Book)
                    .Where(bc => bc.Book.ISBN == ISBN && bc.Status == "Published")
                    .OrderBy(bc => bc.Chapter)
                    .ToListAsync();

                var book = await _db.Books
                    .FirstOrDefaultAsync(b => b.ISBN == ISBN);

                if (book == null || !bookContents.Any())
                {
                    return NotFound("Không tìm thấy nội dung sách.");
                }
                var relatedBooks = _db.Books
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .Where(b => b.ISBN != ISBN &&
                            (b.CategoryId == book.CategoryId || b.AuthorId == book.AuthorId))
                    .Take(5)
                    .ToList();

                // Gửi dữ liệu tới View
                ViewBag.BookTitle = book.Title;
                ViewBag.ISBN = book.ISBN;
                ViewBag.CoverImage = book.CoverImage;
                ViewBag.Views = book.Views;
                ViewBag.Status = book.Status;
                ViewBag.PublishedYear = book.PublishedYear;
                ViewBag.RelatedBooks = relatedBooks;

                return View(bookContents);
            }
            else
            {
                return BadRequest("Bạn chưa thuê sách này.");
            }
        }


    }
}
