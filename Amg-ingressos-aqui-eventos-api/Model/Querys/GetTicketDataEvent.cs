using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model.Querys
{
    public class GetTicketDataEvent : Ticket
    {
        public GetLot Lot { get; set; }
        public GetVariant Variant { get; set; }
        public GetEvent Event { get; set; }
    }
}