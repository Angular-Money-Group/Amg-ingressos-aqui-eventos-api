using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Lot
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
        public string? Id { get; set; }
        /// <summary>
        /// Total de ingressos
        /// </summary>
        [Required]
        [JsonProperty("Description")]
        public string? Description { get; set; }
        /// <summary>
        /// Total de ingressos
        /// </summary>
        [Required]
        [JsonProperty("TotalTickets")]
        public int TotalTickets { get; set; }
        /// <summary>
        /// Total de ingressos
        /// </summary>
        [Required]
        [JsonProperty("ValueTotal")]
        public decimal ValueTotal { get; set; }
        /// <summary>
        /// Data Inicio das vendas
        /// </summary>
        [Required]
        [JsonProperty("StartDateSales")]
        public DateTime StartDateSales { get; set; }
        /// <summary>
        /// Data Fim das vendas
        /// </summary>
        [Required]
         [JsonProperty("EndDateSales")]
        public DateTime EndDateSales { get; set; }
        /// <summary>
        /// Posicoes/cadeiras
        /// </summary>
         [JsonProperty("Positions")]
        public Positions Positions { get; set; }
        /// <summary>
        /// status Lot
        /// </summary>
        [Required]
        [JsonProperty("Status")]
        public Enum.StatusLot Status { get; set; }
        /// <summary>
        /// Id Variant
        /// </summary>
        [Required]
        [JsonProperty("IdVariant")]
        public string IdVariant { get; set; }

    }
}