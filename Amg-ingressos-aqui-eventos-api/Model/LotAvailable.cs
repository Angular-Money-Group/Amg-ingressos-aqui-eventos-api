using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class LotAvailable
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("Idlot")]
        public string? Idlot { get; set; }
        /// <summary>
        /// Identificador de Lote
        /// </summary>
        [Required]
        [JsonProperty("Available")]
        public bool Available { get; set; }
    }
}