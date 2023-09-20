using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Amg_ingressos_aqui_eventos_api.Model.Querys
{

    public partial class GetEvents
    {
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [JsonProperty("Name")]
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

        [JsonProperty("IdOrganizer")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdOrganizer { get; set; }

        [JsonProperty("Variant")]
        public Variant[] Variant { get; set; }

        [JsonProperty("Highlighted")]
        public bool Highlighted { get; set; }

        [JsonProperty("Highlighted")]
        public Enum.StatusEvent Status { get; set; }

        [JsonProperty("Colabs")]
        public List<string>? Colabs { get; set; }

    }

    public partial class Address
    {
        [JsonProperty("Cep")]
        public long Cep { get; set; }

        [JsonProperty("AddressDescription")]
        public string AddressDescription { get; set; }

        [JsonProperty("Number")]
        public long Number { get; set; }

        [JsonProperty("Neighborhood")]
        public string Neighborhood { get; set; }

        [JsonProperty("Complement")]
        public string Complement { get; set; }

        [JsonProperty("ReferencePoint")]
        public string ReferencePoint { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }
    }

    public class Variant
    {
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Positions")]
        public Positions Positions { get; set; }
        [JsonProperty("HasPositions")]
        public bool HasPositions { get; set; }
        [JsonProperty("QuantityCourtesy")]
        public bool QuantityCourtesy { get; set; }

        [JsonProperty("Status")]
        public long Status { get; set; }

        [JsonProperty("IdEvent")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEvent { get; set; }

        [JsonProperty("localeImage")]
        public string? LocaleImage { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Lot")]
        public Lot[] Lot { get; set; }
        [JsonProperty("ReqDocs")]
        public bool ReqDocs { get; set; }

    }

    public class Lot
    {
        [JsonProperty("_id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [JsonProperty("Identificate")]
        public int Identificate { get; set; }

        [JsonProperty("TotalTickets")]
        public long TotalTickets { get; set; }

        [JsonProperty("ValueTotal")]
        public float ValueTotal { get; set; }

        [JsonProperty("StartDateSales")]
        public DateTime StartDateSales { get; set; }

        [JsonProperty("EndDateSales")]
        public DateTime EndDateSales { get; set; }

        [JsonProperty("Status")]
        public long Status { get; set; }

        [JsonProperty("ReqDocs")]
        public bool ReqDocs { get; set; }

        [JsonProperty("IdVariant")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdVariant { get; set; }

    }

    public class Positions
    {
        [JsonProperty("TotalPositions")]
        public int TotalPositions { get; set; }

        [JsonProperty("SoldPositions")]
        public List<int> SoldPositions { get; set; }

        [JsonProperty("ReservedPositions")]
        public List<int> ReservedPositions { get; set; }

        [JsonProperty("PeoplePerPositions")]
        public int PeoplePerPositions { get; set; }
    }
}