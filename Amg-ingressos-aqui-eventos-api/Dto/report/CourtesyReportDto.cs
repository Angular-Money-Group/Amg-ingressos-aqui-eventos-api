using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class CourtesyReportDto
    {
        public CourtesyReportDto()
        {
            Entregues = new DeliveredReportDto();
            Restantes = new RemainingDto();
            IdVariant = string.Empty;
        }

        [JsonPropertyName("entregues")]
        public DeliveredReportDto Entregues { get; set; }

        [JsonPropertyName("restantes")]
        public RemainingDto Restantes { get; set; }
        public string IdVariant { get; set; }
    }
}