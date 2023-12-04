using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class LotReportDto
    {
        public LotReportDto()
        {
            Name = string.Empty;
            AmountTicket = 0;
            Tickets = new TicketsReportDto();
        }

        [JsonPropertyName("Nome")]
        public string Name { get; set; }

        [JsonPropertyName("QuantidadeIngressos")]
        public int AmountTicket { get; set; }

        [JsonPropertyName("Ingressos")]
        public TicketsReportDto Tickets { get; set; }
    }
}