
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LearningService.WebAPI;
using LearningService.WebAPI.Data;
using LearningService.WebAPI.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Xunit;
using LearningService.WebAPI.Data;
using Microsoft.Extensions.DependencyModel;
using HackerRank1.Data;

namespace LearningService.Tests
{    
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly StudentsContext context;

        public HttpClient Client { get; private set; }

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            context = new StudentsContext(new DbContextOptionsBuilder<StudentsContext>()
                        .UseSqlite("DataSource=:memory:")
                        .EnableSensitiveDataLogging()
                        .Options);
            Client = _factory.WithWebHostBuilder(builder =>
                builder.UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(StudentsContext));
                    services.AddSingleton(context);

                    context.Database.OpenConnection();
                    context.Database.EnsureCreated();

                    context.SaveChanges();

                    // Clear local context cache
                    foreach (var entity in context.ChangeTracker.Entries().ToList())
                    {
                        entity.State = EntityState.Detached;
                    }
                })
            ).CreateClient();
        }

        private async Task SeedLibrary()
        {
            var libraries = new List<Activity>
            {
                new Activity { Name = "Library Name 1", Location = "Location 1" },
                new Activity { Name = "Library Name 2", Location = "Location 2" },
                new Activity { Name = "Library Name 3", Location = "Location 3" },
                new Activity { Name = "Library Name 4", Location = "Location 4" }
            };

            await context.Activities.AddRangeAsync(libraries);
            await context.SaveChangesAsync();  // Save to the database
        }

        private async Task SeedStudent(string StudentName, int activityId)
        {
            var StudentForm = new StudentForm
            {
                Name = StudentName,
            };
            var response1 = await Client.PostAsync($"/api/Activity/{activityId}/Students",
                new StringContent(JsonConvert.SerializeObject(StudentForm), Encoding.UTF8, "application/json"));
        }

        // TEST NAME - addStudentToLibrary
        // TEST DESCRIPTION - It adds Student to a quiz
        [Fact]
        public async Task TestAddStudent_Ok_GetStudent_NotFound()
        {
            await SeedLibrary();

            var StudentForm = new StudentForm
            {
                Name = "Test Student 1",
                Id = "1",
                Email = "test"
            };

            var response1 = await Client.PostAsync($"/api/Activity/1/Students",
                new StringContent(JsonConvert.SerializeObject(StudentForm), Encoding.UTF8, "application/json"));

            response1.StatusCode.Should().BeEquivalentTo(StatusCodes.Status201Created);

            StudentForm = new StudentForm
            {
                Name = "Test Student 2",
                Id = "5",
                Email = "test"
            };

            var response2 = await Client.PostAsync($"/api/Activity/100/Students",
                new StringContent(JsonConvert.SerializeObject(StudentForm), Encoding.UTF8, "application/json"));

            response2.StatusCode.Should().BeEquivalentTo(StatusCodes.Status404NotFound);
        }

        // TEST NAME - getStudentsInALibrary
        // TEST DESCRIPTION - It finds all Students in a quiz by ID
        [Fact]
        public async Task TestGetStudents_Ok_NotFound()
        {
            await SeedLibrary();

            //await SeedStudent("test Student 1", 1);
            //await SeedStudent("test Student 2", 1);

            var StudentForm = new StudentForm
            {
                Name = "Test Student 1",
                Id = "1",
                Email = "test"
            };

            var r1 = await Client.PostAsync($"/api/Activity/1/Students",
                new StringContent(JsonConvert.SerializeObject(StudentForm), Encoding.UTF8, "application/json"));

            r1.StatusCode.Should().BeEquivalentTo(StatusCodes.Status201Created);

            StudentForm = new StudentForm
            {
                Name = "Test Student 2",
                Id = "2",
                Email = "test"
            };

            var r2 = await Client.PostAsync($"/api/Activity/1/Students",
                new StringContent(JsonConvert.SerializeObject(StudentForm), Encoding.UTF8, "application/json"));

            r2.StatusCode.Should().BeEquivalentTo(StatusCodes.Status201Created);



            var response1 = await Client.GetAsync($"/api/Activity/2/Students");
            response1.StatusCode.Should().BeEquivalentTo(StatusCodes.Status404NotFound);
            var Students = JsonConvert.DeserializeObject<IEnumerable<Students>>(response1.Content.ReadAsStringAsync().Result).ToList();
            Students.Count.Should().Be(0);

            var response2 = await Client.GetAsync($"/api/Activity/1/Students");
            response2.StatusCode.Should().BeEquivalentTo(StatusCodes.Status200OK);
            var Students2 = JsonConvert.DeserializeObject<IEnumerable<Students>>(response2.Content.ReadAsStringAsync().Result).ToList();
            Students2.Count.Should().Be(2);

            var response3 = await Client.GetAsync($"/api/Activity/31232/Students");
            response3.StatusCode.Should().BeEquivalentTo(StatusCodes.Status404NotFound);
        }

        // TEST NAME - deleteLibraryById
        // TEST DESCRIPTION - Check delete quiz web api end point
        [Fact]
        public async Task TestDeleteLibrary()
        {
            await SeedLibrary();

            var StudentForm = new StudentForm
            {
                Name = "Test Student 1",
                Id = "1",
                Email = "test"
            };

            
            var response0 = await Client.PostAsync("/api/Activity/1/Students",
                new StringContent(JsonConvert.SerializeObject(StudentForm), Encoding.UTF8, "application/json"));
            response0.StatusCode.Should().BeEquivalentTo(StatusCodes.Status201Created);

            
            var response1 = await Client.DeleteAsync("/api/Activity/1");
            response1.StatusCode.Should().BeEquivalentTo(StatusCodes.Status204NoContent);

            
            var response2 = await Client.GetAsync("/api/Activity/1/Students");
            response2.StatusCode.Should().BeEquivalentTo(StatusCodes.Status404NotFound);

            var response3 = await Client.DeleteAsync("/api/Activity/1");
            response3.StatusCode.Should().BeEquivalentTo(StatusCodes.Status404NotFound);
        }
    }
}
