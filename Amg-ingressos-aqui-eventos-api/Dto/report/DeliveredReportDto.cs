using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class DeliveredReportDto
    {
        [JsonPropertyName("Quantidade")]
        public int Amount { get; set; }

        [JsonPropertyName("Percent")]
        public double Percent { get; set; }
    }
}