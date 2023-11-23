using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class CourtesyReportDto
    {
        public CourtesyReportDto()
        {
            Entregues = new DeliveredReportDto();
            Restantes = new RemainingDto();
        }

        [JsonPropertyName("Entregues")]
        public DeliveredReportDto Entregues { get; set; }

        [JsonPropertyName("Restantes")]
        public RemainingDto Restantes { get; set; }
    }
}