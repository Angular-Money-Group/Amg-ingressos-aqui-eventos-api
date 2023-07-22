using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Amg_ingressos_aqui_eventos_api.Model.Querys
{
    public class GetEvent
    {
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Name { get; set; }
        public string Local { get; set; }
        public string Type { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public GetAddress Address { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdMeansReceipt { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdOrganizer { get; set; }
        public bool Highlighted { get; set; }
    }
    public class GetAddress
    {
        public string Cep { get; set; }
        public string AddressDescription { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string Complement { get; set; }
        public string ReferencePoint { get; set; }
        public string City { get; set; }
        public string State { get; set; }

    }
}