using HackerRank1.Data;
using HackerRank1.Enums;
using LearningService.WebAPI.DTO;
using LearningService.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningService.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly IActivitiesService _activitiesService;

    public ActivitiesController(IActivitiesService librariesService)
    {
        _activitiesService = librariesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var libraries = await _activitiesService.Get(null);
        return Ok(libraries);
    }

    [HttpGet("{activityId}")]
    public async Task<IActionResult> Get(int activityId)
    {
        var quiz = (await _activitiesService.Get(new[] { activityId })).FirstOrDefault();
        if (quiz == null)
            return NotFound();
        return Ok(quiz);
    }

    [HttpPost]
    public async Task<IActionResult> Add(ActivityForm l)
    {
        Activity activity = new Activity()
        {            
            Location = l.Location,
            Name = l.Name,            
            ActivityDate = DateTime.Now,
            ActivityType = ActivityTypes.Attendance,
            Status = true,

        };

        await _activitiesService.Add(activity);
        return Ok(l);
    }

    [HttpPut("{activityId}")]
    public async Task<IActionResult> Update(int activityId, Activity quiz)
    {
        var existingQuiz = (await _activitiesService.Get(new[] { activityId })).FirstOrDefault();
        if (existingQuiz == null)
            return NotFound();

        await _activitiesService.Update(quiz);
        return NoContent();
    }

    // Implement the DELETE method below
    [HttpDelete("{activityId}")]
    public async Task<IActionResult> Update(int activityId)
    {
        var existingQuiz = (await _activitiesService.Get(new[] { activityId })).FirstOrDefault();
        if (existingQuiz == null)
            return NotFound();


        await _activitiesService.Delete(existingQuiz);
        return NoContent();
    }

}
