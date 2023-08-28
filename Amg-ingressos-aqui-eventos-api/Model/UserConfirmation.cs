using Microsoft.VisualBasic;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Amg_ingressos_aqui_eventos_api.Model {
    public class UserConfirmation 
    {
        /// <summary>
        /// Confirmação de e-mail
        /// </summary>
        [Required]
        [BsonElement("EmailConfirmationCode")]
        [JsonPropertyName("emailConfirmationCode")]
        public string? EmailConfirmationCode { get; set; }
        
        /// <summary>
        /// Codigo de confirmação de e-mail
        /// </summary>
        [Required]
        [BsonElement("EmailConfirmationExpirationDate")]
        [JsonPropertyName("emailConfirmationExpirationDate")]
        public DateTime? EmailConfirmationExpirationDate { get; set; }

        /// <summary> 
        /// flag de email verificado 
        /// </summary>
        [Required]
        [BsonElement("EmailVerified")]
        [JsonPropertyName("emailVerified")]
        public bool? EmailVerified { get; set; } = false;

        /// <summary> 
        /// flag de telefone verificado 
        /// </summary>
        [Required]
        [BsonElement("PhoneVerified")]
        [JsonPropertyName("phoneVerified")]
        public bool? PhoneVerified { get; set; } = false;
    }
}
