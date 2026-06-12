using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LibraryService.WebAPI;
using LibraryService.WebAPI.Data;
using LibraryService.WebAPI.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Xunit;

namespace LibraryService.Tests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly FraudContext _context;

        public HttpClient Client { get; private set; }

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _context = new FraudContext(new DbContextOptionsBuilder<FraudContext>()
                .UseSqlite("DataSource=:memory:")
                .EnableSensitiveDataLogging()
                .Options);

            Client = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureAppConfiguration((_, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:DefaultConnection"] =
                            "Host=localhost;Database=test;Username=test;Password=test"
                    });
                });
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(FraudContext));
                    services.AddSingleton(_context);

                    _context.Database.OpenConnection();
                    _context.Database.EnsureCreated();

                    _context.Frauds.RemoveRange(_context.Frauds);
                    _context.SaveChanges();

                    foreach (var entity in _context.ChangeTracker.Entries().ToList())
                    {
                        entity.State = EntityState.Detached;
                    }
                });
            }).CreateClient();
        }

        private async Task ClearFraudsAsync()
        {
            _context.Frauds.RemoveRange(_context.Frauds);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task CreateFraudReport_Returns201Created()
        {
            await ClearFraudsAsync();

            var form = new FraudForm
            {
                ImpostorDetails = "Juan Perez",
                ContactInfo = "88888888",
                Comments = "Intento de phishing"
            };

            var response = await Client.PostAsync(
                "/api/fraud",
                new StringContent(JsonConvert.SerializeObject(form), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status201Created);

            var created = JsonConvert.DeserializeObject<Fraud>(
                await response.Content.ReadAsStringAsync());
            created!.ImpostorDetails.Should().Be("Juan Perez");
            created.ContactInfo.Should().Be("88888888");
        }

        [Fact]
        public async Task CreateFraudReport_WithInvalidData_Returns400()
        {
            var form = new FraudForm
            {
                ImpostorDetails = "",
                ContactInfo = ""
            };

            var response = await Client.PostAsync(
                "/api/fraud",
                new StringContent(JsonConvert.SerializeObject(form), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task GetFraudReports_ReturnsAllReports()
        {
            await ClearFraudsAsync();

            var form = new FraudForm
            {
                ImpostorDetails = "Empresa falsa",
                ContactInfo = "correo@test.com",
                Comments = "Reporte de prueba"
            };

            var createResponse = await Client.PostAsync(
                "/api/fraud",
                new StringContent(JsonConvert.SerializeObject(form), Encoding.UTF8, "application/json"));
            createResponse.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status201Created);

            var response = await Client.GetAsync("/api/fraud");
            response.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status200OK);

            var frauds = JsonConvert.DeserializeObject<List<Fraud>>(
                await response.Content.ReadAsStringAsync());
            frauds!.Should().NotBeNull();
            frauds!.Count.Should().BeGreaterOrEqualTo(1);
        }
    }
}
