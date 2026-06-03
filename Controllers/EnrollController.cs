using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.Controllers;

[Authorize(Roles = "STUDENT")]
[Route("enroll/[action]")]
public class EnrollController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public EnrollController(ApplicationDbContext _dbContext, UserManager<IdentityUser> userManager)
    {
        _context = _dbContext;
        _userManager = userManager;
    }

    // POST: enroll/enroll?courseId=5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Enroll(int courseId)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            return Challenge();
        }

        // Verify if course exists
        var courseExists = await _context.Courses.AnyAsync(c => c.Id == courseId);
        if (!courseExists)
        {
            return NotFound("Không tìm thấy học phần.");
        }

        // Verify if already enrolled
        var isEnrolled = await _context.Enrollments
            .AnyAsync(e => e.UserId == userId && e.CourseId == courseId);

        if (!isEnrolled)
        {
            var enrollment = new Enrollment
            {
                UserId = userId,
                CourseId = courseId,
                EnrollDate = DateTime.Now
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();
        }

        return RedirectBack();
    }

    // POST: enroll/cancel?courseId=5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int courseId)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            return Challenge();
        }

        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

        if (enrollment != null)
        {
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
        }

        return RedirectBack();
    }

    // GET: enroll/mycourses
    [HttpGet]
    public async Task<IActionResult> MyCourses()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            return Challenge();
        }

        var enrolledCourses = await _context.Enrollments
            .Where(e => e.UserId == userId)
            .Include(e => e.Course)
                .ThenInclude(c => c!.Category)
            .Select(e => e.Course)
            .ToListAsync();

        return View(enrolledCourses);
    }

    private IActionResult RedirectBack()
    {
        string? referer = Request.Headers["Referer"].ToString();
        if (!string.IsNullOrEmpty(referer))
        {
            return Redirect(referer);
        }
        return RedirectToAction("Index", "Home");
    }
}
