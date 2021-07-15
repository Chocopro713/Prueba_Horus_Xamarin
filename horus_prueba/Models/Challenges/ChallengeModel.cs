namespace horus_prueba.Models.Challenges
{
    using System;
    using Newtonsoft.Json;

    public class ChallengeModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("currentPoints")]
        public long CurrentPoints { get; set; }

        [JsonProperty("totalPoints")]
        public long TotalPoints { get; set; }
    }
}
