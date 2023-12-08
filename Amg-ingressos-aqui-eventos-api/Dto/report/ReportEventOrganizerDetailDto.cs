using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class ReportEventOrganizerDetailDto
    {
        public ReportEventOrganizerDetailDto()
        {
            Name = String.Empty;
            AmountTicket = 0;
            Tickets = new TicketsReportDto();
            Cortesy = new CourtesyReportDto();
            Variant = new VariantReportDto();
        }

        [JsonPropertyName("nome")]
        public string Name { get; set; }

        [JsonPropertyName("quantidadeIngressos")]
        public int AmountTicket { get; set; }

        [JsonPropertyName("ingressos")]
        public TicketsReportDto Tickets { get; set; }

        [JsonPropertyName("cortesias")]
        public CourtesyReportDto Cortesy { get; set; }

        [JsonPropertyName("variant")]
        public VariantReportDto Variant { get; set; }

    }
}