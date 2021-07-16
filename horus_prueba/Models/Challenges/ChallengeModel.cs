namespace horus_prueba.Models.Challenges
{
    using System;
    using System.Diagnostics;
    using Newtonsoft.Json;
    using Prism.Mvvm;

    public class ChallengeModel : BindableBase
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

        // ------------------- //

        public bool IsEqual
        {
            get => this.CurrentPoints == this.TotalPoints;
        }
        public double ProgressBar
        {
            get => Convert.ToDouble(this.CurrentPoints) / Convert.ToDouble(this.TotalPoints);
        }
        public double ProgressPorcent
        {
            get
            {
                var respuesta = (Convert.ToDouble(this.CurrentPoints) / Convert.ToDouble(this.TotalPoints)) * 100;
                respuesta = Math.Round(respuesta, 2);
                return respuesta;
            }
        }
    }
}
