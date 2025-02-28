using Newtonsoft.Json;

namespace LearningService.WebAPI.DTO
{
    public class StudentForm
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("category")]
        public string? GithubUrl { get; set; }

        [JsonProperty("category")]
        public string? Email { get; set; }

        [JsonProperty("activityId")]
        public int activityId { get; set; }        
    }
}
