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

        [JsonPropertyName("nome")]
        public string Name { get; set; }

        [JsonPropertyName("quantidadeIngressos")]
        public int AmountTicket { get; set; }

        [JsonPropertyName("ingressos")]
        public TicketsReportDto Tickets { get; set; }
    }
}