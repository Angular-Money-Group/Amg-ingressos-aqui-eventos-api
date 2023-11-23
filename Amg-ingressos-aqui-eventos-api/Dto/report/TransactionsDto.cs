using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class TransactionsDto
    {
        [JsonPropertyName("Quantidade")]
        public int Amount { get; set; }

        [JsonPropertyName("ValorTotal")]
        public double TotalValue { get; set; }

        [JsonPropertyName("ValorTaxas")]
        public double TaxValue { get; set; }

        [JsonPropertyName("ValorEvento")]
        public double EventValue { get; set; }

        [JsonPropertyName("ValorLiquido")]
        public double LiquidValue { get; set; }
    }
}