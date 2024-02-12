using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class TransactionIten
    {
        public TransactionIten(){
            Id = string.Empty;
            IdTransaction = string.Empty;
            IdTicket = string.Empty;
            TicketPrice = string.Empty;
            Details = string.Empty;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        [JsonProperty("_id")]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdTransaction { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdTicket { get; set; }
        public bool HalfPrice { get; set; }
        public string TicketPrice { get; set; }
        public string Details { get; set; }
    }
}