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
        public string Description { get; set; }
        /// <summary>
        /// CEP
        /// </summary>
        [Required]
        public string Cep { get; set; }
        /// <summary>
        /// Endereço
        /// </summary>
        [Required]
        public string Address { get; set; }
        /// <summary>
        /// Número
        /// </summary>
        [Required]
        public int Number { get; set; }
        /// <summary>
        /// Vizinho
        /// </summary>
        [Required]
        public string Neighborhood { get; set; }
        /// <summary>
        /// Complemento
        /// </summary>
        [Required]
        public string Complement { get; set; }
        /// <summary>
        /// Ponto de referencia
        /// </summary>
        [Required]
        public string ReferencePoint { get; set; }
        /// <summary>
        /// Cidade
        /// </summary>
        [Required]
        public string City { get; set; }
        /// <summary>
        /// Estado
        /// </summary>
        [Required]
        public string State { get; set; }
        /// <summary>
        /// Dia
        /// </summary>
        [Required]
        public string Day { get; set; }
        /// <summary>
        /// Lote
        /// </summary>
        [Required]
        public string Lot { get; set; }
        /// <summary>
        /// Área VIP
        /// </summary>
        [Required]
        public string VipArea { get; set; }
    }
}