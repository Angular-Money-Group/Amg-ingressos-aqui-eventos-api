using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Dto
{
    public class EventEditDto
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
        [JsonPropertyName("id")]
        public string _Id { get; set; } = string.Empty;

        /// <summary>
        /// name
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Local
        /// </summary>
        [Required]
        public string Local { get; set; } = string.Empty;
        /// <summary>
        /// Type
        /// </summary>
        [Required]
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// Image
        /// </summary>
        [Required]
        public string Image { get; set; } = string.Empty;
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
        public Enum.StatusEvent Status { get; set; }

        /// <summary>
        /// Endereço
        /// </summary>
        [Required]
        public Model.Address? Address { get; set; }
        
        /// <summary>
        /// Id mongo Meio de Recebimento
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdMeansReceipt { get; set; } = string.Empty;
        
        /// <summary>
        /// Se o evento está em destaque
        /// </summary>
        [BsonDefaultValue(false)]
        public bool Highlighted { get; set; }
    }
}