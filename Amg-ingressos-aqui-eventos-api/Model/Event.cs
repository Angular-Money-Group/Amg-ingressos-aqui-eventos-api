using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Event
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }

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
        [Required]
        [JsonProperty("Status")]
        public Enum.StatusEvent Status { get; set; }

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