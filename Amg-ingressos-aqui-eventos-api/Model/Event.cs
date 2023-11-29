using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Event
    {
        public Event()
        {
            Id = string.Empty;
            Name = string.Empty;
            Local = string.Empty;
            Type = string.Empty;
            Image = string.Empty;
            Variant = new List<Variant>();
            IdMeansReceipt = string.Empty;
            IdOrganizer = string.Empty;
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
        /// name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Local
        /// </summary>
        [Required]
        public string Local { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        [Required]
        public string Type { get; set; }
        /// <summary>
        /// Image
        /// </summary>
        [Required]
        public string Image { get; set; }
        /// <summary>
        /// Descrição
        /// </summary>
        [Required]
        public string? Description { get; set; }
        /// <summary>
        /// Data Inicio
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Data Fim
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// status Lot
        /// </summary>
        [JsonProperty("Status")]
        public Enum.EnumStatusEvent Status { get; set; }

        /// <summary>
        /// Endereço
        /// </summary>
        [Required]
        public Address? Address { get; set; }

        /// <summary>
        /// Lista de Variants
        /// </summary>
        [BsonIgnore]
        [Required]
        [JsonProperty("Variant")]
        public List<Variant> Variant { get; set; }

        /// <summary>
        /// Lista de Variants
        /// </summary>
        [JsonProperty("courtesy")]
        public Courtesy? Courtesy { get; set; }

        /// <summary>
        /// Id mongo Meio de Recebimento
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdMeansReceipt { get; set; }

        /// <summary>
        /// Id mongo organizador do evento
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdOrganizer { get; set; }

        /// <summary>
        /// Se o evento está em destaque
        /// </summary>
        [BsonDefaultValue(false)]
        public bool Highlighted { get; set; }
    }
}