using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_eventos_api.Model.Querys
{
    public class GetLot
    {
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int Identificate { get; set; }

        public int TotalTickets { get; set; }

        public decimal ValueTotal { get; set; }

        public DateTime StartDateSales { get; set; }

        public DateTime EndDateSales { get; set; }

        public Enum.StatusLot Status { get; set; }

        public bool ReqDocs { get; set; }
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdVariant { get; set; }
    }
}