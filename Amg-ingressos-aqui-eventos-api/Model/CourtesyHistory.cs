using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class CourtesyHistory
    {
        public CourtesyHistory()
        {
            Email = string.Empty;
            IdStatusEmail = string.Empty;
            Variant = string.Empty;
        }

        /// <summary>
        /// Email associated with courtesy history
        /// </summary>
        [BsonElement("Email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// Email associated with courtesy history
        /// </summary>
        [BsonElement("IdStatusEmail")]
        [JsonPropertyName("idStatusEmail")]
        public string IdStatusEmail { get; set; }

        /// <summary>
        /// Courtesy history date
        /// </summary>
        [BsonElement("Date")]
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Variant associated with courtesy history
        /// </summary>
        [BsonElement("Variant")]
        [JsonPropertyName("variant")]
        public string Variant { get; set; }

        /// <summary>
        /// Quantity associated with courtesy history
        /// </summary>
        [BsonElement("Quantity")]
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}