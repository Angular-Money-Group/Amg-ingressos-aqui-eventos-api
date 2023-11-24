namespace Amg_ingressos_aqui_eventos_api.Model.Querys
{
    public class GetTicketDataEvent : Ticket
    {
        public GetTicketDataEvent()
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