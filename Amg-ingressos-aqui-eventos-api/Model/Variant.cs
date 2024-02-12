using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Amg_ingressos_aqui_eventos_api.Dto;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    [BsonIgnoreExtraElements]
    public class Variant
    {
        public Variant()
        {
            Id = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            IdEvent = string.Empty;
            Positions = new Positions();
            LocaleImage = string.Empty;
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
        [BsonRepresentation(BsonType.ObjectId)]

        public string IdEvent { get; set; }
        /// <summary>
        /// Id Evento
        /// </summary>
        [Required]
        [JsonProperty("quantityCourtesy")]
        public int QuantityCourtesy { get; set; }

        /// <summary>
        /// Permitir venda de restante no proximo lote
        /// </summary>
        [JsonProperty("SellTicketsInAnotherBatch")]
        public bool? SellTicketsInAnotherBatch { get; set; }
        /// <summary>
        /// Vender lote antes de iniciar outro 
        /// </summary>
        [JsonProperty("SellTicketsBeforeStartAnother")]
        public bool? SellTicketsBeforeStartAnother { get; set; }

        /// <summary>
        /// Vender lote antes de iniciar outro 
        /// </summary>
        [JsonProperty("localeImage")]
        public string LocaleImage { get; set; }

        /// <summary>
        /// Precisa verificar os documentos?
        /// </summary>
        [JsonProperty("reqDocs")]
        public bool ReqDocs { get; set; }

        /// <summary>
        /// Posicoes/cadeiras
        /// </summary>
        [JsonProperty("Positions")]
        public Positions Positions { get; set; }
    }
}