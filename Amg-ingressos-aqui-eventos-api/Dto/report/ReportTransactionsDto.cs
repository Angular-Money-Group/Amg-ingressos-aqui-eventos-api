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

        [JsonPropertyName("Credito")]
        public TransactionsDto Credit { get; set; }

        [JsonPropertyName("Debito")]
        public TransactionsDto Debit { get; set; }

        [JsonPropertyName("Pix")]
        public TransactionsDto Pix { get; set; }

        [JsonPropertyName("Total")]
        public TransactionsDto Total { get; set; }
    }
}