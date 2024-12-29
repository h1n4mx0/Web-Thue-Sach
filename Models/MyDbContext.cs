using System;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Models
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Authors> Authors { get; set; }
        public virtual DbSet<UserAccounts> UserAccounts { get; set; }
        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Rentals> Rentals { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<RolePermissions> RolePermissions { get; set; }
        public virtual DbSet<Permissions> Permissions { get; set; }
        public virtual DbSet<BookContents> BookContents { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;database=LibraryManager;user=root;password=root;", 
                    new MySqlServerVersion(new Version(8, 0, 23)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Users Table Configuration
            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.DateOfBirth);
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.HasOne(e => e.UserAccounts)
                      .WithOne(e => e.Users)
                      .HasForeignKey<UserAccounts>(e => e.UserId);
            });

            // UserAccounts Table Configuration
            modelBuilder.Entity<UserAccounts>(entity =>
            {
                entity.ToTable("UserAccounts");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(u => u.Role)
                    .WithMany(r => r.UserAccounts)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Cascade); 

                entity.HasMany(ua => ua.Rentals)
                    .WithOne(r => r.UserAccount)
                    .HasForeignKey(r => r.UserAccountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Roles Table Configuration
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(50);

                // Liên kết Roles và UserAccounts
                entity.HasMany(r => r.UserAccounts)
                    .WithOne(ua => ua.Role)
                    .HasForeignKey(ua => ua.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Liên kết Roles và RolePermissions
                entity.HasMany(r => r.RolePermissions)
                    .WithOne(rp => rp.Role)
                    .HasForeignKey(rp => rp.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Permissions Table Configuration
            modelBuilder.Entity<Permissions>(entity =>
            {
                entity.ToTable("Permissions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            // RolePermissions Table Configuration
            modelBuilder.Entity<RolePermissions>(entity =>
            {
                entity.ToTable("RolePermissions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PermissionId).IsRequired();

                entity.HasOne(rp => rp.Role)
                      .WithMany(r => r.RolePermissions)
                      .HasForeignKey(rp => rp.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rp => rp.Permission)
                      .WithMany()
                      .HasForeignKey(rp => rp.PermissionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            // Authors Table Configuration
            modelBuilder.Entity<Authors>(entity =>
            {
                entity.ToTable("Authors");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Bio);
            });

            // Categories Table Configuration
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            // Books Table Configuration
            modelBuilder.Entity<Books>(entity =>
            {
                entity.ToTable("Books");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.ISBN).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.ISBN).IsUnique();
                entity.Property(e => e.PublishedYear);
                entity.Property(e => e.Summary);
                entity.Property(e => e.CoverImage).HasMaxLength(255);
                entity.Property(e => e.RentalPrice).HasMaxLength(50);
                entity.Property(e => e.ReadingDuration).HasMaxLength(50);
                entity.Property(e => e.Views).HasDefaultValue(0);
                entity.Property(e => e.Status).HasDefaultValue("Đã hoàn thành")
                      .HasColumnType("enum('Đã hoàn thành', 'Đang cập nhật')");

                entity.HasOne(d => d.Category)
                      .WithMany(p => p.Books)
                      .HasForeignKey(d => d.CategoryId);

                entity.HasOne(d => d.Author)
                      .WithMany(p => p.Books)
                      .HasForeignKey(d => d.AuthorId);
            });

            // Rentals Table Configuration
            modelBuilder.Entity<Rentals>(entity =>
            {
                entity.ToTable("Rentals");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RentalDate).IsRequired();
                entity.Property(e => e.ReturnDate).IsRequired();
                entity.Property(e => e.RentalStatus).HasDefaultValue("Đang thuê")
                      .HasColumnType("enum('Đang thuê', 'Hết hạn')");

                entity.HasOne(e => e.UserAccount)
                      .WithMany(ua => ua.Rentals)
                      .HasForeignKey(e => e.UserAccountId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Book)
                      .WithMany(b => b.Rentals)
                      .HasForeignKey(e => e.ISBN)
                      .HasPrincipalKey(b => b.ISBN)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BookContents>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookContent)
                .HasForeignKey(bc => bc.BookId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
