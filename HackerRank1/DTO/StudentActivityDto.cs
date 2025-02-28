using Newtonsoft.Json;

namespace HackerRank1.DTO
{
    public class StudentActivityDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public DateTime ActivityDate { get; set; }

        [JsonProperty("status")]
        public bool Status { get; set; }
    }
}
