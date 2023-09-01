using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class ReportEventOrganizerDto
    {
        public string Nome { get; set; }
        public int QuantidadeIngressos { get; set; }
        public IngressosReportDto Ingressos { get; set; }
        public CortesiasReportDto Cortesias { get; set; }
    }
}