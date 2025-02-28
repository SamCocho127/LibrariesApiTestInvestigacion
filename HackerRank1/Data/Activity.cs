using HackerRank1.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackerRank1.Data;

public class Activity
{
    [Key]    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    [Required]
    [MaxLength(255)]
    public string Location { get; set; }

    [Required]
    public ActivityTypes ActivityType { get; set; }

    [Required]
    public DateTime ActivityDate { get; set; }

    [Required]
    public bool Status { get; set; }

    // Navigation property for many-to-many relationship
    public ICollection<StudentActivity> StudentActivities { get; set; } = new List<StudentActivity>();
}