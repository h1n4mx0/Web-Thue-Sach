using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using LibraryManager.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Middlewares
{
    public class RentalStatusMiddleware
    {
        private readonly RequestDelegate _next;

        public RentalStatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, MyDbContext dbContext)
        {
            // Check if the user is authenticated
            if (context.User.Identity.IsAuthenticated)
            {
                // Retrieve User ID from the claims (stored in ClaimTypes.NameIdentifier)
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (int.TryParse(userIdClaim, out int userId))
                {
                    // Get list of overdue rentals for this user
                    var expiredRentals = await dbContext.Rentals
                        .Where(r => r.UserAccountId == userId && r.RentalStatus == "Đang thuê" && r.ReturnDate < DateTime.Now)
                        .ToListAsync();

                    // Update the status from "Đang thuê" to "Hết hạn" for overdue rentals
                    foreach (var rental in expiredRentals)
                    {
                        rental.RentalStatus = "Hết hạn";
                    }

                    // Save changes to the database if there are any expired rentals
                    if (expiredRentals.Any())
                    {
                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            // Pass the request to the next middleware
            await _next(context);
        }
    }
}
