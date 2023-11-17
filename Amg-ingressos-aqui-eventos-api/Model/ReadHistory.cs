using Amg_ingressos_aqui_eventos_api.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class ReadHistory
    {
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
        public string idEvent { get; set; }

        /// <summary>
        /// Id do colaborador que leu o ticket
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string idColab { get; set; }
        /// <summary>
        /// Id do ticket (qrcode)
        /// </summary>
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string idTicket { get; set; }

        /// <summary>
        /// Data leitura
        /// </summary>
        [Required]
        public DateTime date { get; set; }
        /// <summary>
        /// Status do ticket
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// Observação leitura
        /// </summary>
        public string reason { get; set; }
    }
}
