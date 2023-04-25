using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Variant
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        /// <summary>
        /// Nome Variant
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Lista de lotes
        /// </summary>
        [BsonIgnore]
        public List<Lot> Lot { get; set; }
        /// <summary>
        /// Flag Posicoes
        /// </summary>
        [Required]
        public bool Positions { get; set; }
        /// <summary>
        /// status variante
        /// </summary>
        [Required]
        public Enum.StatusVariant Status { get; set; }
        /// <summary>
        /// Id Evento
        /// </summary>
        [Required]
        public string IdEvent { get; set; }
    }
}