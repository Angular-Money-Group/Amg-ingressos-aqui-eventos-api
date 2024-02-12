using System.Transactions;
using Amg_ingressos_aqui_eventos_api.Dto;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class EventComplet : Event
    {
        public EventComplet()
        {
            Variants = new List<Variant>();
            Lots = new List<Lot>();
            User = new List<User>();
            Tickets = new List<Ticket>();
            Transactions = new List<Transaction>();
        }
        public List<Variant> Variants { get; set; }
        public List<Lot> Lots { get; set; }
        public List<User> User { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}