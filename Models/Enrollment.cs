using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Test.Models;

public class Enrollment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public int CourseId { get; set; }

    [Required]
    public DateTime EnrollDate { get; set; } = DateTime.Now;

    [ForeignKey("CourseId")]
    public Course? Course { get; set; }

    [ForeignKey("UserId")]
    public IdentityUser? User { get; set; }
}
