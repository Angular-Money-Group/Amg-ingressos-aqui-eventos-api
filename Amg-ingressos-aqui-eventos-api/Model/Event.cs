using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    [BsonIgnoreExtraElements]
    public class Event
    {
        public Event()
        {
            Id = string.Empty;
            Name = string.Empty;
            Local = string.Empty;
            Type = string.Empty;
            Image = string.Empty;
            Description = string.Empty;
            Address = new Address();
            Courtesy = new Courtesy();
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
        public string Description { get; set; }
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
        public Enum.EnumStatusEvent Status { get; set; }

        /// <summary>
        /// Endereço
        /// </summary>
        [Required]
        public Address Address { get; set; }

        /// <summary>
        /// Lista de Variants
        /// </summary>
        public Courtesy Courtesy { get; set; }

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

        public void ValidateModelSave()
        {
            if (this.Name == "")
                throw new SaveException("Nome é Obrigatório.");
            if (this.Local == "")
                throw new SaveException("Local é Obrigatório.");
            if (this.Type == "")
                throw new SaveException("Tipo é Obrigatório.");
            if (this.Description == "")
                throw new SaveException("Descrição é Obrigatório.");
            if (this.Address == null)
                throw new SaveException("Endereço é Obrigatório.");
            if (this.Address.Cep == "")
                throw new SaveException("CEP é Obrigatório.");
            if (this.Address.Number == string.Empty)
                throw new SaveException("Número Endereço é Obrigatório.");
            if (this.Address.Neighborhood == "")
                throw new SaveException("Vizinhança é Obrigatório.");
            if (this.Address.City == "")
                throw new SaveException("Cidade é Obrigatório.");
            if (this.Address.State == "")
                throw new SaveException("Estado é Obrigatório.");
            if (this.StartDate == DateTime.MinValue)
                throw new SaveException("Data Inicio é Obrigatório.");
            if (this.EndDate == DateTime.MinValue)
                throw new SaveException("Data Fim é Obrigatório.");
            
            validateImage(this.Image);
        }

        private void validateImage(string image)
        {
            if (string.IsNullOrEmpty(image))
                throw new SaveException("Imagem é obrigatório");
            
            image.IsBase64String();

            try
            {
                var base64Data = Regex
                .Match(image, @"data:image/(?<type>.+?),(?<data>.+)")
                .Groups["data"].Value;
                Convert.FromBase64String(base64Data);
            }
            catch (FormatException)
            {
                throw new SaveException("Essa imagem não está em base64");
            }
        }
    }
}