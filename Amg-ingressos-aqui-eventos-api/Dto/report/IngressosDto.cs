using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class IngressosReportDto
    {
        public VendidosDto Vendidos { get; set; }
        public RestantesDto Restantes { get; set; }
    }
}