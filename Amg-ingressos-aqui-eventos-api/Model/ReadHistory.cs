using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class ReadHistory
    {
        public ReadHistory()
        {
            Id = string.Empty;
            IdEvent = string.Empty;
            IdColab = string.Empty;
            IdTicket = string.Empty;
            Reason = string.Empty;
        }

        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
        public string Id { get; set; }

        /// <summary>
        /// Id do evento
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEvent { get; set; }

        /// <summary>
        /// Id do colaborador que leu o ticket
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdColab { get; set; }
        /// <summary>
        /// Id do ticket (qrcode)
        /// </summary>

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdTicket { get; set; }

        /// <summary>
        /// Data leitura
        /// </summary>
        [Required]
        public DateTime Date { get; set; }
        /// <summary>
        /// Status do ticket
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Observação leitura
        /// </summary>
        public string Reason { get; set; }
    }
}