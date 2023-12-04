using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class ReportEventOrganizerDto
    {
        public ReportEventOrganizerDto()
        {
            Name = string.Empty;
            AmountTicket = 0;
            Tickets = new TicketsReportDto();
            Cortesys = new CourtesyReportDto();
        }

        [JsonPropertyName("Nome")]
        public string Name { get; set; }

        [JsonPropertyName("QuantidadeIngressos")]
        public int AmountTicket { get; set; }

        [JsonPropertyName("Ingressos")]
        public TicketsReportDto Tickets { get; set; }

        [JsonPropertyName("Cortesias")]
        public CourtesyReportDto Cortesys { get; set; }
    }
}