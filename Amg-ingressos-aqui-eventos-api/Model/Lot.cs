using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    [BsonIgnoreExtraElements]
    public class Lot
    {
        public Lot()
        {
            Id = string.Empty;
            IdVariant = string.Empty;
        }

        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Identificador de Lote
        /// </summary>
        [Required]
        [JsonProperty("Identificate")]
        public int Identificate { get; set; }

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
        /// status Lot
        /// </summary>
        [Required]
        [JsonProperty("Status")]
        public Enum.StatusLot Status { get; set; }

        /// <summary>
        /// Precisa verificar os documentos?
        /// </summary>
        [JsonProperty("reqDocs")]
        public bool ReqDocs { get; set; }

        /// <summary>
        /// Id Variant
        /// </summary>
        [Required]
        [JsonProperty("IdVariant")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdVariant { get; set; }
    }
}