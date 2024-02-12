using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class DeliveredReportDto
    {
        [JsonPropertyName("quantidade")]
        public int Amount { get; set; }

        [JsonPropertyName("percent")]
        public double Percent { get; set; }
    }
}