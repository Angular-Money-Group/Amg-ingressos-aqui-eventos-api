using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    [BsonIgnoreExtraElements]
    public class StatusTicketsRow
    {
        public StatusTicketsRow()
        {
            Id = string.Empty;
            Email = string.Empty;
            TicketStatus = new List<TicketStatusResult>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [BsonElement("Email")]
        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [BsonElement("TotalTickets")]
        [JsonPropertyName("totalTickets")]
        public int TotalTickets { get; set; }

        [BsonElement("TicketStatus")]
        [JsonPropertyName("ticketStatus")]
        public List<TicketStatusResult> TicketStatus { get; set; }
    }
}