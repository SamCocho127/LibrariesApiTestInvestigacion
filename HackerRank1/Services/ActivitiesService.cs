using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackerRank1.Data;
using LearningService.WebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace LearningService.WebAPI.Services
{
    public class ActivitiesService : IActivitiesService
    {
        private readonly StudentsContext _quizContext;

        public ActivitiesService(StudentsContext quizContext)
        {
            _quizContext = quizContext;
        }

        public async Task<IEnumerable<Activity>> Get(int[] ids)
        {
            var projects = _quizContext.Activities.AsQueryable();

            if (ids != null && ids.Any())
                projects = projects.Where(x => ids.Contains(x.Id));

            return await projects.ToListAsync();
        }

        public async Task<Activity> Add(Activity quiz)
        {
            await _quizContext.Activities.AddAsync(quiz);

            await _quizContext.SaveChangesAsync();
            return quiz;
        }

        public async Task<IEnumerable<Activity>> AddRange(IEnumerable<Activity> projects)
        {
            await _quizContext.Activities.AddRangeAsync(projects);
            await _quizContext.SaveChangesAsync();
            return projects;
        }

        public async Task<Activity> Update(Activity quiz)
        {
            var projectForChanges = await _quizContext.Activities.SingleAsync(x => x.Id == quiz.Id);
            projectForChanges.Name = quiz.Name;
            projectForChanges.Location = quiz.Location;

            _quizContext.Activities.Update(projectForChanges);
            await _quizContext.SaveChangesAsync();
            return quiz;
        }

        public async Task<bool> Delete(Activity quiz)
        {
            Activity currentLib = await _quizContext.Activities.Where(x => x.Id == quiz.Id).FirstOrDefaultAsync();

            
            _quizContext.Activities.Remove(currentLib);
            return await _quizContext.SaveChangesAsync() > 0;
        }
    }

    public interface IActivitiesService
    {
        Task<IEnumerable<Activity>> Get(int[] ids);

        Task<Activity> Add(Activity quiz);

        Task<Activity> Update(Activity quiz);

        Task<bool> Delete(Activity quiz);
    }
}
