using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Contact
    {
        public Contact()
        {
            Email = string.Empty;
            PhoneNumber = string.Empty;
        }

        /// <summary>
        /// E-mail de validação 
        /// </summary> 
        [BsonElement("Email")]
        [JsonPropertyName("Email")]
        public string Email { get; set; }

        /// <summary>
        /// Número para contato 
        /// </summary>    
        [BsonElement("PhoneNumber")]
        [JsonPropertyName("PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}