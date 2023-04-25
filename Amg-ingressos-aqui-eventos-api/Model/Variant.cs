using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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
        [Required]
        public List<Lot> Lot { get; set; }
        /// <summary>
        /// Flag Posicoes
        /// </summary>
        [Required]
        public bool positions { get; set; }
    }
}