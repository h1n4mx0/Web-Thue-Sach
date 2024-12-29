using System.Collections.Generic;

namespace LibraryManager.Models
{
    public partial class Books
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public string ISBN { get; set; }
        public int? PublishedYear { get; set; }
        public string Summary { get; set; }
        public string CoverImage { get; set; }
        public string RentalPrice { get; set; } // Example format: "20,000 VNĐ"
        public string ReadingDuration { get; set; } // Example format: "5h-30m"
        public int Views { get; set; }
        public string Status { get; set; } // "Đã hoàn thành" or "Đang cập nhật"

        // Navigation properties
        public virtual Categories Category { get; set; }
        public virtual Authors Author { get; set; }
        public virtual ICollection<Rentals> Rentals { get; set; } = new List<Rentals>(); // Add Rentals navigation property
        public virtual ICollection<BookContents> BookContent { get; set; } = new List<BookContents>(); // Added Chapters navigation property
    }
}
