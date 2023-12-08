using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class TransactionsDto
    {
        [JsonPropertyName("quantidade")]
        public int Amount { get; set; }

        [JsonPropertyName("valorTotal")]
        public double TotalValue { get; set; }

        [JsonPropertyName("valorTaxas")]
        public double TaxValue { get; set; }

        [JsonPropertyName("valorEvento")]
        public double EventValue { get; set; }

        [JsonPropertyName("valorLiquido")]
        public double LiquidValue { get; set; }
    }
}