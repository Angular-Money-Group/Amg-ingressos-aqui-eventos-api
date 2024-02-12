using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class TicketEventDataDto : Ticket
    {
        public TicketEventDataDto()
        {
            Lot = new Lot();
            Variant = new Variant();
            Event = new Event();
        }

        public Lot Lot { get; set; }
        public Variant Variant { get; set; }
        public Event Event { get; set; }
    }
}