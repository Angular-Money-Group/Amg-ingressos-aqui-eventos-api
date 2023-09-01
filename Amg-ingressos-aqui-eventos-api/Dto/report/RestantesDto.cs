using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Dto.report
{
    public class RestantesDto
    {
        public int Quantidade { get; set; }
        public double Percent { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal Taxa { get; set; }
        public decimal ValorReceber { get; set; }
    }
}