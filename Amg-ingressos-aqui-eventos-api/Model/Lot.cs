using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Lot
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        /// <summary>
        /// Total de ingressos
        /// </summary>
        [Required]
        public string? Description { get; set; }
        /// <summary>
        /// Total de ingressos
        /// </summary>
        [Required]
        public int TotalTickets { get; set; }
        /// <summary>
        /// Total de ingressos
        /// </summary>
        [Required]
        public decimal ValueTotal { get; set; }
        /// <summary>
        /// Data Inicio das vendas
        /// </summary>
        [Required]
        public DateTime StartDateSales { get; set; }
        /// <summary>
        /// Data Fim das vendas
        /// </summary>
        [Required]
        public DateTime EndDateSales { get; set; }
        /// <summary>
        /// Posicoes/cadeiras
        /// </summary>
        public Positions positions { get; set; }

    }
}