using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Variant
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
        public string? Id { get; set; }
        /// <summary>
        /// Nome Variant
        /// </summary>
        [Required]
        [JsonProperty("Name")]
        public string Name { get; set; }
        /// <summary>
        /// Flag Posicoes
        /// </summary>
        [Required]
        [JsonProperty("Positions")]
        public bool Positions { get; set; }
        /// <summary>
        /// status variante
        /// </summary>
        [Required]
        [JsonProperty("Status")]
        public Enum.StatusVariant Status { get; set; }
        /// <summary>
        /// Id Evento
        /// </summary>
        [Required]
        [JsonProperty("IdEvent")]
        public string IdEvent { get; set; }
        /// <summary>
        /// Lista de lotes
        /// </summary>
        [BsonIgnore]
        [JsonProperty("Lot")]
        public List<Lot> Lot { get; set; }
    }
}