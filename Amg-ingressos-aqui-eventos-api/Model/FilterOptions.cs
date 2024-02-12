using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class FilterOptions
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("local")]
        public string? Local { get; set; }

        [JsonProperty("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("idOrganizer")]
        public string? IdOrganizer { get; set; }

        [JsonProperty("highlights")]
        public bool? Highlights { get; set; }

        [JsonProperty("highlights")]
        public string? Status { get; set; }

    }
}