using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class ReportTransactionsDto
    {
        public ReportTransactionsDto()
        {
            Credit = new TransactionsDto();
            Debit = new TransactionsDto();
            Pix = new TransactionsDto();
            Total = new TransactionsDto();
        }

        [JsonPropertyName("credito")]
        public TransactionsDto Credit { get; set; }

        [JsonPropertyName("debito")]
        public TransactionsDto Debit { get; set; }

        [JsonPropertyName("pix")]
        public TransactionsDto Pix { get; set; }

        [JsonPropertyName("total")]
        public TransactionsDto Total { get; set; }
    }
}