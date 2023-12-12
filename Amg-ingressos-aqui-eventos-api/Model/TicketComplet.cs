

using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    [BsonIgnoreExtraElements]
    public class TicketComplet : Ticket
    {
        public TicketComplet()
        {
            Lots = new List<Lot>();
            Variants = new List<Variant>();
            Events = new List<Event>();
            Users = new List<User>();
        }

        public List<Lot> Lots { get; set; }
        public List<Variant> Variants { get; set; }
        public List<Event> Events { get; set; }
        public List<User> Users { get; set; }
    }
}