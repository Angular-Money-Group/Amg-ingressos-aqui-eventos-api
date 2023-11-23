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

        [JsonPropertyName("Nome")]
        public string Name { get; set; }

        [JsonPropertyName("QuantidadeIngressos")]
        public int AmountTicket { get; set; }

        [JsonPropertyName("Ingressos")]
        public TicketsReportDto Tickets { get; set; }

        [JsonPropertyName("Cortesias")]
        public CourtesyReportDto Cortesy { get; set; }

        [JsonPropertyName("Variant")]
        public VariantReportDto Variant { get; set; }

    }
}