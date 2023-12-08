using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class RemainingDto
    {
        [JsonPropertyName("quantidade")]
        public int Amount { get; set; }

        [JsonPropertyName("percent")]
        public double Percent { get; set; }

        [JsonPropertyName("valorTotal")]
        public decimal TotalValue { get; set; }

        [JsonPropertyName("taxa")]
        public decimal Tax { get; set; }

        [JsonPropertyName("valorReceber")]
        public decimal ReceiveValue { get; set; }
    }
}