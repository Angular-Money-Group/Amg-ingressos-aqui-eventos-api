using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Event
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        /// <summary>
        /// name
        /// </summary>
        [Required]
        public string? Name { get; set; }
        /// <summary>
        /// Local
        /// </summary>
        [Required]
        public string? Local { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        [Required]
        public string? Type { get; set; }
        /// <summary>
        /// Image
        /// </summary>
        [Required]
        public string? Image { get; set; }
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
        /// Endereço
        /// </summary>
        [Required]
        public Address? Address { get; set; }
        /// <summary>
        /// Lista de Variants
        /// </summary>
        [BsonIgnore]
        [Required]
        public List<Variant>? Variant { get; set; }
        /// <summary>
        /// Id mongo Meio de Recebimento
        /// </summary>
        public string? IdMeansReceipt { get; set; }
        /// <summary>
        /// Id mongo Meio de Recebimento
        /// </summary>
        public string? IdOrganizer { get; set; }
    }
}