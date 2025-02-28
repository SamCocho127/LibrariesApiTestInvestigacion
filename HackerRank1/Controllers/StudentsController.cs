using HackerRank1.Data;
using HackerRank1.DTO;
using LearningService.WebAPI.DTO;
using LearningService.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningService.WebAPI.Controllers;

[ApiController]
[Route("api/Activity/{activityId}/[Controller]")]
public class StudentsController : ControllerBase
{
    private readonly IActivitiesService _QuizzesService;
    private readonly IStudentsService _StudentsService;

    public StudentsController(IStudentsService StudentsService, IActivitiesService librariesService)
    {
        _QuizzesService = librariesService;
        _StudentsService = StudentsService;
    }

    // Implement the functionalities below
    [HttpPost]
    public async Task<IActionResult> AddStudent(int activityId, [FromBody] StudentForm StudentForm)
    {
        Activity currentQuiz = (await _QuizzesService.Get(new int[] { activityId })).FirstOrDefault();
        if (currentQuiz == null)
            return NotFound();

        Students Student = new Students()
        {
            Id = StudentForm.Id,
            Email = StudentForm.Email,
            Name = StudentForm.Name,
            activityId = activityId                
        };

        return StatusCode(201, await _StudentsService.Add(Student));
    }

    [HttpGet]
    public async Task<IActionResult> GetStudents(int activityId)
    {
        IEnumerable<StudentActivityDto> Students =  await _StudentsService.Get(activityId, new int[] { activityId });
        
        return  Students is null  || Students.Count() == 0 ? NotFound(Students) : Ok(Students);
    }
}