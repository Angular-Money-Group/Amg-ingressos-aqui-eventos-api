using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Ticket
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        /// <summary>
        /// Id mongo Evento
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdEvent { get; set; }
        /// <summary>
        /// Id mongo Variante
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdVariant { get; set; }
        /// <summary>
        /// Id mongo Lote
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdLote { get; set; }
        /// <summary>
        /// Posicao
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Position { get; set; }
        /// <summary>
        /// Valor Ingresso
        /// </summary>
        public decimal Value { get; set; }
    }
}