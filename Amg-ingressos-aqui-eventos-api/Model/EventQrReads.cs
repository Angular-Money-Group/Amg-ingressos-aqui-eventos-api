using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class EventQrReads
    {
        public EventQrReads()
        {
            Id = string.Empty;
            IdEvent = string.Empty;
            IdColab = string.Empty;
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
        /// Id do evento
        /// </summary>
        public string IdEvent { get; set; }
        /// <summary>
        /// Id do colaborador
        /// </summary>
        public string IdColab { get; set; }
        /// <summary>
        /// Total de qrcode(ticket) lidos
        /// </summary>
        public int TotalReads { get; set; }
        /// <summary>
        /// Total lido com sucesso
        /// </summary>
        public int TotalSuccess { get; set; }
        /// <summary>
        /// Total lido com falha
        /// </summary>
        public int TotalFail { get; set; }
        /// <summary>
        /// Data inicial da leitura
        /// </summary>
        public DateTime InitialDate { get; set; }
        /// <summary>
        /// Data ultima leitura
        /// </summary>
        public DateTime LastRead { get; set; }
        /// <summary>
        /// Status da leitura
        /// </summary>
        public int? Status { get; set; }
    }
}