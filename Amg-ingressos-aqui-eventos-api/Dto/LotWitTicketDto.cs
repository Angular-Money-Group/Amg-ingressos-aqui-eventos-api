using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class LotWithTicketDto : Lot
    {
        public LotWithTicketDto()
        {
            Tickets = new List<Ticket>();
        }
        public List<Ticket> Tickets { get; set; }
    }
}