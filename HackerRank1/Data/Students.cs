using System.ComponentModel.DataAnnotations;

namespace HackerRank1.Data;


public class Students
{
    [Key]
    public string Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? GithubUrl { get; set; }

    public int activityId { get; set; }
}