using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Courtesy
    {
        /// <summary>
        /// Remaining courtesy quantity
        /// </summary>
        [BsonElement("RemainingCourtesyQuantity")]
        [JsonPropertyName("remainingCourtesyQuantity")]
        public List<RemainingCourtesy> RemainingCourtesy { get; set; }

        /// <summary>
        /// List of courtesy history
        /// </summary>
        [BsonElement("CourtesyHistory")]
        [JsonPropertyName("courtesyHistory")]
        public List<CourtesyHistory> CourtesyHistory { get; set; }
    }

    public class CourtesyHistory
    {
        /// <summary>
        /// Email associated with courtesy history
        /// </summary>
        [BsonElement("Email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

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
    
    public class RemainingCourtesy
    {
        /// <summary>
        /// Remaining courtesy quantity
        /// </summary>
        [BsonElement("Quantity")]
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Courtesy history date
        /// </summary>
        [BsonElement("Variant")]
        [JsonPropertyName("variant")]
        public string Variant { get; set; }
    }
}
