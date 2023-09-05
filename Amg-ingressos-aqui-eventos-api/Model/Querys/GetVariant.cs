using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Amg_ingressos_aqui_eventos_api.Model.Querys
{
    public class GetVariant
    {
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasPositions { get; set; }
        public int Status { get; set; }
          public int QuantityCourtesy { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEvent { get; set; }
        public bool ReqDocs { get; set; }
        public IList<object> Positions { get; set; }
    
    }
}