using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace LibraryManager.Areas.Admin.Models
{
    public class EditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề.")]
        public string Title { get; set; } 
        [Required(ErrorMessage = "Vui lòng chọn tác giả.")]
        public int AuthorId { get; set; } 
        [Required(ErrorMessage = "Vui lòng chọn thể loại.")]
        public int CategoryId { get; set; } 
        [Required(ErrorMessage = "Vui lòng nhập ISBN.")]
        public string ISBN { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập năm xuất bản.")]
        [Range(1900, 2100, ErrorMessage = "Năm xuất bản không hợp lệ.")]
        public int PublishedYear { get; set; } 
        public string Summary { get; set; } 
        public string CoverImage { get; set; } 
        [Required(ErrorMessage = "Vui lòng nhập giá thuê.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá thuê không hợp lệ.")]
        public decimal RentalPrice { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thời gian đọc.")]
        [Range(1, int.MaxValue, ErrorMessage = "Thời gian đọc không hợp lệ.")]
        public int ReadingDuration { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn trạng thái.")]
        public string Status { get; set; }
    }
}
