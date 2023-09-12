using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Pagination
    {
        /// <summary>
        /// Pagina atual
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// Tamanho da Pagina
        /// </summary>
        public int pageSize { get; set; }
    }

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
    }
}
