using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using Amg_ingressos_aqui_eventos_api.Enum;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    [BsonIgnoreExtraElements]
    public class User
    {  
        public User()
        {
            Id = string.Empty;
            Name = string.Empty;
            DocumentId = string.Empty;
            Address = new Address();
            Contact = new Contact();
            UserConfirmation = new UserConfirmation();
            Password = string.Empty;
            IdAssociate = string.Empty;
            Sex = string.Empty;
            BirthDate = string.Empty;
        }

        /// <summary>
        /// Id do usuário
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }
        /// <summary>
        /// name
        /// </summary>
        [BsonElement("Name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <sumary>
        /// Documento identificação
        /// </sumary>
        [BsonElement("DocumentId")]
        [JsonPropertyName("documentId")]
        public string DocumentId { get; set; }

        /// <sumary>
        /// Status
        /// </sumary>
        [BsonElement("Status")]
        [JsonPropertyName("status")]
        public EnumTypeStatus Status { get; set; }

        /// <summary>
        /// Tipo do usuário
        /// </summary>
        [BsonElement("Type")]
        [JsonPropertyName("type")]
        public EnumTypeUser Type { get; set; }

        /// <summary>
        /// Endereço do usuário
        /// </summary>
        [BsonElement("Address")]
        [JsonPropertyName("address")]
        public Address Address { get; set; }

        /// <summary>
        /// Contato do usuário
        /// </summary>
        [BsonElement("Contact")]
        [JsonPropertyName("contact")]
        public Contact Contact { get; set; }

        /// <summary>
        /// Confirmação do usuário
        /// </summary>
        [BsonElement("UserConfirmation")]
        [JsonPropertyName("userConfirmation")]
        public UserConfirmation UserConfirmation { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [BsonElement("Password")]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [BsonElement("idAssociate")]
        [JsonPropertyName("idAssociate")]
        public string IdAssociate { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [BsonElement("updatedAt")]
        [JsonPropertyName("updatedAt")]
        public DateTime updatedAt { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [BsonElement("UpdateAt")]
        [JsonPropertyName("UpdateAt")]
        public DateTime? UpdateAt { get; set; }
         /// <summary>
        /// Sexo
        /// </summary>
        [JsonPropertyName("sex")]
        public string Sex { get; set; }

        /// <summary>
        /// data nacimento
        /// </summary>
        [JsonPropertyName("birthDate")]
        public string BirthDate { get; set; }
    }
}