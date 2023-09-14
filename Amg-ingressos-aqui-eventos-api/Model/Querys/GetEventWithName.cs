using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_eventos_api.Model.Querys
{

    public partial class GetEventsWithNames
    {
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("Local")]
        public string Local { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Image")]
        public string Image { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("StartDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("EndDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty("Address")]
        public Address Address { get; set; }

        [JsonProperty("Courtesy")]
        public Courtesy? Courtesy { get; set; }

        [JsonProperty("IdMeansReceipt")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdMeansReceipt { get; set; }
        
        [JsonProperty("ReqDocs")]
        public bool ReqDocs { get; set; }

        [JsonProperty("IdOrganizer")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdOrganizer { get; set; }

        [JsonProperty("Highlighted")]
        public bool Highlighted { get; set; }

        [JsonProperty("Highlighted")]
        public Enum.StatusEvent Status { get; set; }

        [JsonProperty("User")]
        public Producer User { get; set; }
    };

    public class Producer
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}