using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Amg_ingressos_aqui_eventos_api.Model.Querys
{
    public class GetTicketDataUser : Ticket
    {
        public List<User> User { get; set; }
        public List<GetLot> Lot { get; set; }
    }
}