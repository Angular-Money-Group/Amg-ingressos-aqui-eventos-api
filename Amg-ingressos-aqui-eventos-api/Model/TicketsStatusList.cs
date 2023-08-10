using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    [BsonIgnoreExtraElements]
    public class StatusTicketsRow
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
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

    public class TicketStatusResult
    {
        [BsonElement("Identificate")]
        [JsonPropertyName("identificate")]
        public int Identificate { get; set; }

        [BsonElement("Status")]
        [JsonPropertyName("status")]
        public TicketStatusEnum Status { get; set; }

        [BsonElement("TicketId")]
        [JsonPropertyName("ticketId")]
        public string TicketId { get; set; }

        [BsonElement("Message")]
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }

    public enum TicketStatusEnum
    {
        Processando = 0,
        Enviado = 1,
        Erro = 2
    }
}
