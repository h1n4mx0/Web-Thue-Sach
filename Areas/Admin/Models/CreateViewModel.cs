using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LibraryManager.Areas.Admin.Models
{
    public class CreateViewModel
    {
        public string Title { get; set; }
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public string ISBN { get; set; }
        public int? PublishedYear { get; set; }
        public string Summary { get; set; }
        public IFormFile CoverImage { get; set; }
        public string RentalPrice { get; set; }
        public string ReadingDuration { get; set; }
        public string Status { get; set; } // "Đã hoàn thành" or "Đang cập nhật"
    }

}
