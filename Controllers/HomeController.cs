using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(
        ILogger<HomeController> logger, 
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string? searchString, int page = 1)
    {
        if (page < 1) page = 1;
        
        ViewData["CurrentFilter"] = searchString;
        
        int pageSize = 5;
        var query = _context.Courses.Include(c => c.Category).AsQueryable();
        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(c => c.Name.Contains(searchString));
        }
        
        int totalItems = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        
        // If the page is out of range, default to page 1
        if (page > totalPages && totalPages > 0)
        {
            page = totalPages;
        }

        var courses = await query
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var viewModel = new HomeViewModel
        {
            Courses = courses,
            PageIndex = page,
            TotalPages = totalPages
        };

        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var userId = _userManager.GetUserId(User);
            if (!string.IsNullOrEmpty(userId))
            {
                viewModel.EnrolledCourseIds = await _context.Enrollments
                    .Where(e => e.UserId == userId)
                    .Select(e => e.CourseId)
                    .ToListAsync();
            }
        }

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
