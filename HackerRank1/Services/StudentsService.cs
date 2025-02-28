using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningService.WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using HackerRank1.Data;
using HackerRank1.DTO;

namespace LearningService.WebAPI.Services
{
    public class StudentsService : IStudentsService
    {
        private readonly StudentsContext _studentContext;

        public StudentsService(StudentsContext quizContext)
        {
            _studentContext = quizContext;
        }

        public async Task<IEnumerable<StudentActivityDto>> Get(int activityId, int[] ids)
        {
            // Complete the implementation
            IEnumerable<Students> Students = await _studentContext.Students.Where(x => x.activityId == activityId).ToListAsync();
            Activity activity = await _studentContext.Activities.Where(x => x.Id == activityId).FirstOrDefaultAsync();


            IEnumerable<StudentActivityDto> studentActivities = Students.Select(x => new StudentActivityDto() { 
                Id = x.Id,
                ActivityDate = activity.ActivityDate,
                Name = x.Name,
                Status = activity.Status
            });

            return studentActivities;
        }

        public async Task<bool> Add(Students Student)
        {
            await _studentContext.Students.AddAsync(Student);
            await _studentContext.SaveChangesAsync();

            return true;
        }

        public async Task<Students> Update(Students Student)
        {
            // Complete the implementation
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(Students Student)
        {
            // Complete the implementation
            throw new NotImplementedException();
        }
    }

    public interface IStudentsService
    {
        Task<IEnumerable<StudentActivityDto>> Get(int activityId, int[] ids);

        Task<bool> Add(Students Student);

        Task<Students> Update(Students Student);

        Task<bool> Delete(Students Student);
    }
}
