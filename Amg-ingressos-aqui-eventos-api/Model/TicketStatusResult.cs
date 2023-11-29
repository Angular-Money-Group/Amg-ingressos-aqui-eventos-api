using System.Text.Json.Serialization;
using Amg_ingressos_aqui_eventos_api.Enum;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class TicketStatusResult
    {
        public TicketStatusResult()
        {
            TicketId = string.Empty;
            Message = string.Empty;
        }

        [BsonElement("Identificate")]
        [JsonPropertyName("identificate")]
        public int Identificate { get; set; }

        [BsonElement("Status")]
        [JsonPropertyName("status")]
        public EnumTicketStatusProcess Status { get; set; }

        [BsonElement("TicketId")]
        [JsonPropertyName("ticketId")]
        public string TicketId { get; set; }

        [BsonElement("Message")]
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}