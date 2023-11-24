using Amg_ingressos_aqui_eventos_api.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model
{
    public class Ticket
    {
        public Ticket()
        {
            Id = string.Empty;
            IdLot = string.Empty;
            IdUser = string.Empty;
            QrCode = string.Empty;
            Position = string.Empty;
            IdColab = string.Empty;
        }

        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
        public string Id { get; set; }

        /// <summary>
        /// Id mongo Lote
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdLot { get; set; }

        /// <summary>
        /// Id mongo Usuário
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUser { get; set; }

        /// <summary>
        /// Posicao
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Valor Ingresso
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Se o ingresso já foi vendido
        /// </summary>
        [BsonDefaultValue(false)]
        public bool IsSold { get; set; }

        /// <summary>
        /// STATUS DO INGRESSO
        /// </summary>
        public StatusTicket? Status { get; set; }

        /// <summary>
        /// Colaborador que realizou a leitura
        /// </summary>
        public string IdColab { get; set; }

        /// <summary>
        /// Precisa verificar os documentos?
        /// </summary>
        public bool ReqDocs { get; set; }

        /// <summary>
        /// QrCode
        /// </summary>
        public string QrCode { get; set; }
        /// <summary>
        /// Indica se o ticket é cortesia
        /// </summary>
        public Boolean TicketCortesia { get; set; }
    }
}