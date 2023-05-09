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
        /// Description Variante
        /// </summary>
        [Required]
        [JsonProperty("Description")]
        public string Description { get; set; }
        /// <summary>
        /// Flag Posicoes
        /// </summary>
        [Required]
        [JsonProperty("Positions")]
        public bool HasPositions { get; set; }
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
        /// <summary>
        /// Permitir venda de restante no proximo lote
        /// </summary>
        [BsonIgnore]
        [JsonProperty("SellTicketsInAnotherBatch")]
        public bool SellTicketsInAnotherBatch { get; set; }
        /// <summary>
        /// Vender lote antes de iniciar outro 
        /// </summary>
        [BsonIgnore]
        [JsonProperty("SellTicketsBeforeStartAnother")]
        public bool SellTicketsBeforeStartAnother { get; set; }
        /// <summary>
        /// Vender lote antes de iniciar outro 
        /// </summary>
        [BsonIgnore]
        [JsonProperty("localeImage")]
        public string LocaleImage { get; set; }

        /// <summary>
        /// Posicoes/cadeiras
        /// </summary>
         [JsonProperty("Positions")]
        public Positions Positions { get; set; }
        
    }
}