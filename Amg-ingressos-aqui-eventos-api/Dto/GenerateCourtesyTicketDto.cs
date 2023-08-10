using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class GenerateCourtesyTicketDto
    {
        public string Email { get; set; }
        public string IdVariant { get; set; }
        public int quantity { get; set; }
    }
}