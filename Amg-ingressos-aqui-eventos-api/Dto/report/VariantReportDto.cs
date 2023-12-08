using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class VariantReportDto
    {
        public VariantReportDto()
        {
            Name = string.Empty;
            AmountTickets = 0;
            Tickets = new TicketsReportDto();
            Cortesys = new CourtesyReportDto();
            Lots = new List<LotReportDto>();
        }

        [JsonPropertyName("nome")]
        public string Name { get; set; }

        [JsonPropertyName("quantidadeIngressos")]
        public int AmountTickets { get; set; }

        [JsonPropertyName("ingressos")]
        public TicketsReportDto Tickets { get; set; }

        [JsonPropertyName("cortesias")]
        public CourtesyReportDto Cortesys { get; set; }

        [JsonPropertyName("lotes")]
        public List<LotReportDto> Lots { get; set; }
    }
}