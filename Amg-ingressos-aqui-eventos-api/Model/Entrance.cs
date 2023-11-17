using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Entrance
    {
        /// <summary>
        /// Id mongo Usuário
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUser { get; set; }

        /// <summary>
        /// Id Evento
        /// </summary>
        [Required]
        [JsonProperty("IdEvent")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEvent { get; set; }

        /// <summary>
        /// Id Ticket
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdTicket { get; set; }
    }
}
