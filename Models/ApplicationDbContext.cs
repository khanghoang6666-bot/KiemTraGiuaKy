using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Test.Models;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Roles
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "8c819ea2-2ea9-42b7-a3cf-e83cb4dfd8ea", Name = "STUDENT", NormalizedName = "STUDENT" },
            new IdentityRole { Id = "778c1871-3323-4554-aa61-ad8d4a9cf291", Name = "ADMIN", NormalizedName = "ADMIN" }
        );

        // Seed Admin User
        var hasher = new PasswordHasher<IdentityUser>();
        var adminUser = new IdentityUser
        {
            Id = "admin-user-id-seeded",
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@test.com",
            NormalizedEmail = "ADMIN@TEST.COM",
            EmailConfirmed = true,
            SecurityStamp = string.Empty
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@123");

        modelBuilder.Entity<IdentityUser>().HasData(adminUser);

        // Link Admin User to Admin Role
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = "778c1871-3323-4554-aa61-ad8d4a9cf291",
            UserId = "admin-user-id-seeded"
        });

        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Công nghệ phần mềm" },
            new Category { Id = 2, Name = "Khoa học máy tính" },
            new Category { Id = 3, Name = "An toàn thông tin" }
        );

        // Seed Courses
        modelBuilder.Entity<Course>().HasData(
            new Course
            {
                Id = 1,
                Name = "Lập trình C# cơ bản",
                Image = "https://images.unsplash.com/photo-1517694712202-14dd9538aa97?w=500&auto=format&fit=crop&q=60",
                Credits = 3,
                Lecturer = "TS. Nguyễn Văn A",
                CategoryId = 1
            },
            new Course
            {
                Id = 2,
                Name = "Cấu trúc dữ liệu và giải thuật",
                Image = "https://images.unsplash.com/photo-1605379399642-870262d3d051?w=500&auto=format&fit=crop&q=60",
                Credits = 4,
                Lecturer = "ThS. Trần Thị B",
                CategoryId = 2
            },
            new Course
            {
                Id = 3,
                Name = "Lập trình Web với ASP.NET Core",
                Image = "https://images.unsplash.com/photo-1555066931-4365d14bab8c?w=500&auto=format&fit=crop&q=60",
                Credits = 3,
                Lecturer = "TS. Lê Hoàng C",
                CategoryId = 1
            },
            new Course
            {
                Id = 4,
                Name = "Cơ sở dữ liệu",
                Image = "https://images.unsplash.com/photo-1544383835-bda2bc66a55d?w=500&auto=format&fit=crop&q=60",
                Credits = 3,
                Lecturer = "PGS.TS. Phạm Văn D",
                CategoryId = 2
            },
            new Course
            {
                Id = 5,
                Name = "Phân tích và thiết kế hệ thống",
                Image = "https://images.unsplash.com/photo-1504639725590-34d0984388bd?w=500&auto=format&fit=crop&q=60",
                Credits = 3,
                Lecturer = "ThS. Vũ Minh E",
                CategoryId = 1
            },
            new Course
            {
                Id = 6,
                Name = "Nhập môn An toàn thông tin",
                Image = "https://images.unsplash.com/photo-1526374965328-7f61d4dc18c5?w=500&auto=format&fit=crop&q=60",
                Credits = 3,
                Lecturer = "TS. Đặng Quang F",
                CategoryId = 3
            },
            new Course
            {
                Id = 7,
                Name = "Trí tuệ nhân tạo",
                Image = "https://images.unsplash.com/photo-1531297484001-80022131f5a1?w=500&auto=format&fit=crop&q=60",
                Credits = 4,
                Lecturer = "GS.TS. Nguyễn Hải G",
                CategoryId = 2
            },
            new Course
            {
                Id = 8,
                Name = "Mạng máy tính",
                Image = "https://images.unsplash.com/photo-1558494949-ef010cbdcc31?w=500&auto=format&fit=crop&q=60",
                Credits = 3,
                Lecturer = "ThS. Trịnh Văn H",
                CategoryId = 3
            },
            new Course
            {
                Id = 9,
                Name = "Phát triển ứng dụng di động",
                Image = "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?w=500&auto=format&fit=crop&q=60",
                Credits = 3,
                Lecturer = "ThS. Bùi Thị I",
                CategoryId = 1
            },
            new Course
            {
                Id = 10,
                Name = "Mẫu thiết kế phần mềm",
                Image = "https://images.unsplash.com/photo-1531403009284-440f080d1e12?w=500&auto=format&fit=crop&q=60",
                Credits = 3,
                Lecturer = "TS. Hoàng Văn J",
                CategoryId = 1
            }
        );
    }
}
