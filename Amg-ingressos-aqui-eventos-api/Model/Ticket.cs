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
        /// Id mongo Lote
        /// </summary>
        public string? IdLot { get; set; }
        /// <summary>
        /// Posicao
        /// </summary>
        public string? Position { get; set; }
        /// <summary>
        /// Valor Ingresso
        /// </summary>
        public decimal Value { get; set; }
    }
}