using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Amg_ingressos_aqui_eventos_api.Model.Querys.GetEventwithTicket
{
    public class GetEventWitTickets
    {
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
        public Address Address { get; set; }
        public Courtesy Courtesy { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdMeansReceipt { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdOrganizer { get; set; }
        public bool Highlighted { get; set; }
        public List<Variant> Variant { get; set; }
    }
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Address
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

    public class Courtesy
    {
        public List<RemainingCourtesy> RemainingCourtesy { get; set; }
        public List<CourtesyHistory> CourtesyHistory { get; set; }
    }

    public class Lot
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public int Identificate { get; set; }
        public int TotalTickets { get; set; }
        public decimal ValueTotal { get; set; }
        public DateTime StartDateSales { get; set; }
        public DateTime EndDateSales { get; set; }
        public int Status { get; set; }
        public bool ReqDocs { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdVariant { get; set; }
        public List<Ticket> ticket { get; set; }
    }

    public class RemainingCourtesy
    {
        public int Quantity { get; set; }
        public string VariantName { get; set; }
        public string VariantId { get; set; }
    }
    public class CourtesyHistory
    {
        public string Email { get; set; }
        public string IdStatusEmail { get; set; }
        public DateTime Date { get; set; }
        public string Variant { get; set; }
        public int Quantity { get; set; }
    }

    public class Ticket
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdLot { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUser { get; set; }
        public object Position { get; set; }
        public decimal Value { get; set; }
        public bool isSold { get; set; }
        public object Status { get; set; }
        public object IdColab { get; set; }
        public bool ReqDocs { get; set; }
        public object QrCode { get; set; }
    }

    public class Variant
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasPositions { get; set; }
        public int Status { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEvent { get; set; }
        public int QuantityCourtesy { get; set; }
        public bool ReqDocs { get; set; }
        public object Positions { get; set; }
        public List<Lot> Lot { get; set; }
        public string? LocaleImage { get; set; }
    }
}