using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class TicketsReportDto
    {
        public TicketsReportDto()
        {
            Sold = new SoldDto();
            Remaining = new RemainingDto();
        }

        [JsonPropertyName("vendidos")]
        public SoldDto Sold { get; set; }

        [JsonPropertyName("restantes")]
        public RemainingDto Remaining { get; set; }
    }
}