using System.Collections.Generic;

namespace Test.Models;

public class HomeViewModel
{
    public List<Course> Courses { get; set; } = new List<Course>();
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public List<int> EnrolledCourseIds { get; set; } = new List<int>();

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}
