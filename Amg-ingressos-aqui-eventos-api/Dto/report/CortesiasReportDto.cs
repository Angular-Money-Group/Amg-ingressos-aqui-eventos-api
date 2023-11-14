using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class CortesiasReportDto
    {
        [JsonPropertyName("Entregues")]
        public EntreguesReportDto Entregues { get; set; }

        [JsonPropertyName("Restantes")]
        public RestantesDto Restantes { get; set; }

        public CortesiasReportDto(){
            Entregues = new EntreguesReportDto();
            Restantes = new RestantesDto();
        }
    }
}