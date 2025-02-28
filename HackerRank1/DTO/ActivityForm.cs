using HackerRank1.Enums;
using Newtonsoft.Json;

namespace LearningService.WebAPI.DTO
{
    public class ActivityForm
    {    
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }
    }
}
