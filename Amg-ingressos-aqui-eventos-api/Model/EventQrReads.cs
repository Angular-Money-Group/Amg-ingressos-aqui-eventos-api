using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class EventQrReads
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }
        /// <summary>
        /// Id do evento
        /// </summary>
        public string idEvent { get; set; }
        /// <summary>
        /// Id do colaborador
        /// </summary>
        public string idColab { get; set; }
        /// <summary>
        /// Total de qrcode(ticket) lidos
        /// </summary>
        public int totalReads { get; set; }
        /// <summary>
        /// Total lido com sucesso
        /// </summary>
        public int totalSuccess { get; set; }
        /// <summary>
        /// Total lido com falha
        /// </summary>
        public int totalFail { get; set; }
        /// <summary>
        /// Data inicial da leitura
        /// </summary>
        public DateTime initialDate { get; set; }
        /// <summary>
        /// Data ultima leitura
        /// </summary>
        public DateTime? lastRead { get; set; }
        /// <summary>
        /// Status da leitura
        /// </summary>
        public int? status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> loginHistory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> readHistory { get; set; }
    }
}
