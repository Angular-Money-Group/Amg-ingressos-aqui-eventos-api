using Amg_ingressos_aqui_eventos_api.Model;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class LotWitTicketDto : Lot
    {
        public LotWitTicketDto()
        {
            Tickets = new List<Ticket>();
        }
        List<Ticket> Tickets { get; set; }
    }
}