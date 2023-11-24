using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Courtesy
    {
        public Courtesy()
        {
            RemainingCourtesy = new List<RemainingCourtesy>();
            CourtesyHistory = new List<CourtesyHistory>();
        }

        /// <summary>
        /// Remaining courtesy quantity
        /// </summary>
        [BsonElement("RemainingCourtesy")]
        [JsonPropertyName("RemainingCourtesy")]
        public List<RemainingCourtesy> RemainingCourtesy { get; set; }

        /// <summary>
        /// List of courtesy history
        /// </summary>
        [BsonElement("CourtesyHistory")]
        [JsonPropertyName("courtesyHistory")]
        public List<CourtesyHistory> CourtesyHistory { get; set; }
    }
}