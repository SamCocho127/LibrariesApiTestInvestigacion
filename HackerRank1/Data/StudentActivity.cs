using System.ComponentModel.DataAnnotations;

namespace HackerRank1.Data;

public class StudentActivity
{    
    [Key]
    public Guid StudentActivityId { get; set; }

    [Required]
    public DateTime AttendanceDate { get; set; } = DateTime.UtcNow;

    // foreign key properties
    public string StudentId { get; set; }
    public int ActivityId { get; set; }

    // Navigation properties
    public Students Student { get; set; }

    public Activity Activity { get; set; }
}

