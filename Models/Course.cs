using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Models;

public class Course
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Image { get; set; }

    [Required]
    [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10.")]
    public int Credits { get; set; }

    [Required]
    [StringLength(150)]
    public string Lecturer { get; set; } = string.Empty;

    [Required]
    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
}
