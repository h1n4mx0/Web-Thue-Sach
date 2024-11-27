using System;
using System.Linq;
using System.Threading.Tasks;
using LibraryManager.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Services
{
    public class RentalService
    {
        private readonly MyDbContext _dbContext;

        public RentalService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Hàm kiểm tra sách quá hạn và cập nhật trạng thái
        public async Task CheckAndUpdateOverdueRentalsAsync(int userAccountId)
        {
            var overdueRentals = await _dbContext.Rentals
                .Where(r => r.UserAccountId == userAccountId && r.ReturnDate <= DateTime.Now && r.RentalStatus == "Đang thuê")
                .ToListAsync();

            foreach (var rental in overdueRentals)
            {
                rental.RentalStatus = "Hết hạn";
            }

            if (overdueRentals.Any())
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        // Thêm các hàm khác liên quan đến nghiệp vụ thuê sách
    }
}
