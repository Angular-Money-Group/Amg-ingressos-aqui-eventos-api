using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class VariantReportDto
    {
        public VariantReportDto()
        {
            Name = String.Empty;
            AmountTickets = 0;
            Tickets = new TicketsReportDto();
            Cortesys = new CourtesyReportDto();
            Lots = new List<LotReportDto>();
        }

        [JsonPropertyName("Nome")]
        public string Name { get; set; }

        [JsonPropertyName("QuantidadeIngressos")]
        public int AmountTickets { get; set; }

        [JsonPropertyName("Ingressos")]
        public TicketsReportDto Tickets { get; set; }

        [JsonPropertyName("Cortesias")]
        public CourtesyReportDto Cortesys { get; set; }

        [JsonPropertyName("Lotes")]
        public List<LotReportDto> Lots { get; set; }
    }
}