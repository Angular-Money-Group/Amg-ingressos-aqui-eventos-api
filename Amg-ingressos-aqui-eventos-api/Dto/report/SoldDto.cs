using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class SoldDto
    {
        [JsonPropertyName("Quantidade")]
        public int Amount { get; set; }

        [JsonPropertyName("Percent")]
        public double Percent { get; set; }

        [JsonPropertyName("ValorTotal")]
        public decimal TotalValue { get; set; }

        [JsonPropertyName("Taxa")]
        public decimal Tax { get; set; }

        [JsonPropertyName("ValorReceber")]
        public decimal ReceiveValue { get; set; }
    }
}